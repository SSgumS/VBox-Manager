using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VBox_Manager.Models;

namespace VBox_Manager.Data
{
    public class VBox_ManagerContext : DbContext
    {
        public VBox_ManagerContext (DbContextOptions<VBox_ManagerContext> options)
            : base(options)
        {
        }

        public DbSet<Vm> Vm { get; set; }
    }
}
