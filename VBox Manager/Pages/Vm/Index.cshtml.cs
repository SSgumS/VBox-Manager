using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VBox_Manager.Services;

namespace VBox_Manager.Pages.Vm
{
    public class IndexModel : PageModel
    {
        private readonly VBoxManager _manager;

        public IndexModel(VBoxManager manager)
        {
            _manager = manager;
        }

        public IEnumerable<Models.Vm> Vms { get; set; }

        public async Task OnGetAsync()
        {
            Vms = await _manager.GetVmsAsync();
        }

        public async Task OnPostStartAsync(Guid id, string name)
        {
            try
            {
                await _manager.StartVmAsync(id);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, $"Can't start VM {name}.");
            }

            await OnGetAsync();
        }

        public async Task OnPostStopAsync(Guid id, string name)
        {
            try
            {
                await _manager.StopVmAsync(id);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, $"Can't stop VM {name}.");
            }

            await OnGetAsync();
        }

        public async Task OnPostCloneAsync(Guid id, string name)
        {
            try
            {
                await _manager.CloneVmAsync(id, $"{name} Clone");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, $"Can't clone VM {name}.");
            }

            await OnGetAsync();
        }

        public async Task OnPostDeleteAsync(Guid id, string name)
        {
            try
            {
                await _manager.DeleteVmAsync(id);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, $"Can't delete VM {name}.");
            }

            await OnGetAsync();
        }
    }
}