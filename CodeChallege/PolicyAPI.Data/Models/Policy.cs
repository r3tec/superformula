using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PolicyAPI.Data.Models
{
    public class Policy
    {
        public long Id { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string DriverLicenseNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public double Premium { get; set; }

        public Vehicle Vehicle { get; set; } 
        
    }
}
