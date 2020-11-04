using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VBox_Manager.Models;

namespace VBox_Manager.Services
{
    public class VBoxManager
    {
        public IEnumerable<Vm> GetVms()
        {
            return new Vm[] {
                new Vm { Id = Guid.NewGuid(), CpuNumber = 1, Memory = 2048, Name = "Temp1" },
                new Vm { Id = Guid.NewGuid(), CpuNumber = 2, Memory = 4096, Name = "Temp2" }
            };
        }

        public Vm GetVm(Guid guid)
        {
            return new Vm { Id = Guid.NewGuid(), CpuNumber = 1, Memory = 2048, Name = "Temp" };
        }

        /// <summary>
        /// Edit vm.
        /// </summary>
        /// <param name="cpuNumber"></param>
        /// <param name="memory"></param>
        /// <returns>Returns true if edit was successful.</returns>
        /// <exception cref="Exception">When edit throw an exception.</exception>
        public bool EditVm(int cpuNumber, int memory)
        {
            return true;
        }

        public string RunCommand(string command)
        {
            return "Done";
        }
    }
}
