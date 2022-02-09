using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyAPI.Data.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string VehicleName { get; set; }
        public Policy AttachedPolicy { get; set; }

    }
}
