using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace VBox_Manager.Extensions
{
    public static class PhysicalAddressExtensions
    {
        public static string ToString(this PhysicalAddress address, string separator)
        {
            return string.Join(separator, address.GetAddressBytes().Select(x => x.ToString("X2")));
        }
    }
}
