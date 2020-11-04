using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VBox_Manager.Services;

namespace VBox_Manager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VBoxManager _manager;

        public IndexModel(ILogger<IndexModel> logger, VBoxManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        public IEnumerable<Models.Vm> Vms { get; set; }

        public void OnGet()
        {
            Vms = _manager.GetVms();
        }
    }
}
