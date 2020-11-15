using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VBox_Manager.Services;

namespace VBox_Manager.Pages.Vm
{
    public class SshModel : PageModel
    {
        private readonly VBoxManager _manager;

        public SshModel(VBoxManager manager)
        {
            _manager = manager;
        }

        [BindProperty]
        public Models.Vm Vm { get; set; }
        [BindProperty]
        [Required]
        public string Command { get; set; }
        public string Result { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            Vm = await _manager.GetVmAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Result = await _manager.RunCommandAsync(Vm.Id, Command);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Can't connect to the VM.");
            }

            return Page();
        }
    }
}