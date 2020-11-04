using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VBox_Manager.Models
{
    public class Vm
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public VmStatus Status { get; set; }
        public int CpuNumber { get; set; }
        public int Memory { get; set; }
    }

    public enum VmStatus
    {
        Offline,
        Online
    }
}
