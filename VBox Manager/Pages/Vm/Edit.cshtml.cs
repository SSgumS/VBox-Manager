using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VBox_Manager.Data;
using VBox_Manager.Services;

namespace VBox_Manager.Pages.Vm
{
    public class EditModel : PageModel
    {
        private readonly VBoxManager _manager;

        public EditModel(VBoxManager manager)
        {
            _manager = manager;
        }

        [BindProperty]
        public Models.Vm EditingVm { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var vm = await _manager.GetVmAsync(id);

            if (vm == null)
            {
                return NotFound();
            }

            EditingVm = vm;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _manager.EditVmAsync(EditingVm.Id, EditingVm.Name, EditingVm.CpuNumber, EditingVm.Memory);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, $"Can't modify the VM.");
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
