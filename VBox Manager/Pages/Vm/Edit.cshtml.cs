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
        public Vm EditingVm { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var vm = _manager.GetVm(id);

            if (vm == null)
            {
                return NotFound();
            }

            EditingVm = new Vm { Id = vm.Id, CpuNumber = vm.CpuNumber, Memory = vm.Memory };

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _manager.EditVm(EditingVm.CpuNumber, EditingVm.Memory);
            }
            catch (Exception)
            {
                if (!VmExists(EditingVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Index");
        }

        private bool VmExists(Guid id)
        {
            return _manager.GetVm(id) != null;
        }

        public class Vm
        {
            [Required]
            public Guid Id { get; set; }
            public int CpuNumber { get; set; }
            public int Memory { get; set; }
        }
    }
}
