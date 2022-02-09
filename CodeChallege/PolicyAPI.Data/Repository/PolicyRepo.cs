using PolicyAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyAPI.Data.Repository
{
    public interface IPolicyRepo
    {
        Task<Policy> GetById(long id, string license);
        Task<Policy> Add(Policy p);
        string ConnectionString { get; }
    }

    public class PolicyRepo : IPolicyRepo
    {
        public string ConnectionString
        {
            get { return dbContext.DbPath; }
        }
        private PolicyContext dbContext { get; set; }
        public PolicyRepo(PolicyContext pc)
        {
            dbContext = pc;
        }

        public async Task<Policy> GetById(long id, string license)
        {
            var policy = await dbContext.Policies.FindAsync(id);
            return policy?.DriverLicenseNumber.Equals(license) == true ? policy : null;
        }

        public async Task<Policy> Add(Policy p)
        {
            dbContext.Policies.Add(p);
            var res = await dbContext.SaveChangesAsync();
            if (res < 1)
                return null;
            return dbContext.Policies.Find(p.Id);
        }

    }
}
