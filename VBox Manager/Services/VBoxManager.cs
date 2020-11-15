using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.EntityFrameworkCore;
using VBox_Manager.Models;
using VBox_Manager.Extensions;
using System.Net;
using Renci.SshNet;
using Renci.SshNet.Async;

namespace VBox_Manager.Services
{
    public class VBoxManager
    {
        private Command VBoxManagerProcess { get; set; }

        public VBoxManager()
        {
            VBoxManagerProcess = Cli.Wrap(@"C:\Program Files\Oracle\VirtualBox\VBoxManage.exe");
        }

        private List<Vm> ParseVms(string input)
        {
            var names = Regex.Matches(input, @"^Name: {24}(.+?)\r?$", RegexOptions.Multiline)
                .Select(m => m.Groups[1].Value).ToArray();
            var uuids = Regex.Matches(input, @"^UUID: {24}(.+?)\r?$", RegexOptions.Multiline)
                .Select(m => new Guid(m.Groups[1].Value)).ToArray();
            var cpuNums = Regex.Matches(input, @"^Number of CPUs: {14}(.+?)\r?$", RegexOptions.Multiline)
                .Select(m => int.Parse(m.Groups[1].Value)).ToArray();
            var memories = Regex.Matches(input, @"^Memory size {18}(.+?)MB\r?$", RegexOptions.Multiline)
                .Select(m => int.Parse(m.Groups[1].Value)).ToArray();
            var states = Regex.Matches(input, @"^State: {23}(.+?) \(.+$", RegexOptions.Multiline)
                .Select(m => m.Groups[1].Value == "running" ? VmStatus.Online : VmStatus.Offline).ToArray();

            var vms = new List<Vm>();
            for (int i = 0; i < names.Count(); i++)
            {
                var vm = new Vm
                {
                    Id = uuids[i],
                    Name = names[i],
                    CpuNumber = cpuNums[i],
                    Memory = memories[i],
                    Status = states[i]
                };
                vms.Add(vm);
            }

            return vms;
        }

        public async Task<IEnumerable<Vm>> GetVmsAsync()
        {
            var result = await VBoxManagerProcess.WithArguments("list -l vms").ExecuteBufferedAsync();
            return ParseVms(result.StandardOutput);
        }

        public async Task<Vm> GetVmAsync(Guid guid)
        {
            var result = await VBoxManagerProcess.WithArguments($"showvminfo {guid}").ExecuteBufferedAsync();
            return ParseVms(result.StandardOutput).First();
        }

        public async Task StartVmAsync(Guid guid)
        {
            await VBoxManagerProcess.WithArguments($"startvm {guid} --type=headless").ExecuteAsync();
        }

        public async Task StopVmAsync(Guid guid)
        {
            await VBoxManagerProcess.WithArguments($"controlvm {guid} poweroff").ExecuteAsync();
        }

        public async Task CloneVmAsync(Guid guid, string name)
        {
            await VBoxManagerProcess.WithArguments($"clonevm {guid} --register --name=\"{name}\"").ExecuteAsync();
        }

        public async Task DeleteVmAsync(Guid guid)
        {
            await VBoxManagerProcess.WithArguments($"unregistervm {guid} --delete").ExecuteAsync();
        }

        public async Task EditVmAsync(Guid guid, string name, int cpuNumber, int memory)
        {
            await VBoxManagerProcess.WithArguments($"modifyvm {guid} --name \"{name}\" --memory {memory} --cpus {cpuNumber}").ExecuteAsync();
        }

        public async Task<string> RunCommandAsync(Guid guid, string command)
        {
            var vmDetails = await VBoxManagerProcess.WithArguments($"showvminfo {guid}").ExecuteBufferedAsync();
            var mac = PhysicalAddress.Parse(
                Regex.Match(
                    vmDetails.StandardOutput,
                    @"^NIC 1:.+MAC: (\w+)",
                    RegexOptions.Multiline)
                .Groups[1].Value);
            var arpOutput = await Cli.Wrap("arp").WithArguments("-a").ExecuteBufferedAsync();
            var ip = IPAddress.Parse(
                Regex.Match(
                    arpOutput.StandardOutput,
                    @$"(\d+\.\d+\.\d+\.\d+) +{mac.ToString("-")}",
                    RegexOptions.IgnoreCase | RegexOptions.RightToLeft)
                .Groups[1].Value);
            using var sshClient = new SshClient(ip.ToString(), "ssgums", new PrivateKeyFile[] { new PrivateKeyFile("./Assets/id_rsa") });
            sshClient.Connect();
            var sshCommand = sshClient.CreateCommand(command);
            var result = await sshCommand.ExecuteAsync();
            sshClient.Disconnect();
            return result;
        }
    }
}
