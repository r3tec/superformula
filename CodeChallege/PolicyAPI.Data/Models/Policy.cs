using System;
using System.Collections.Generic;
using System.Text;

namespace PolicyAPI.Data.Models
{
    public class Policy
    {
        public long Id { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string Address { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Premium { get; set; }

        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        
    }
}
