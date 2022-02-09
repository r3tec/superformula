using Microsoft.EntityFrameworkCore;
using PolicyAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyAPI.Data.Repository
{
    public enum SortOrder { None, Asc, Desc};
    public interface IPolicyRepo
    {
        Task<Policy> GetById(long id, string license);
        Task<List<Policy>> GetAll(string license, SortOrder sortOrder, bool returnExpired);
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

        public async Task<List<Policy>> GetAll(string license, SortOrder sortOrder, bool returnExpired)
        {
            
            var policies = await dbContext.Policies
                .Include("Vehicle")
                .Where(p => p.DriverLicenseNumber.Equals(license)).ToListAsync();
            // a bit kludgy, for more complex scenarious can use System.Linq.Expressions.Expression to ad optional predicates
            if (!returnExpired) policies = policies.Where(p => p.ExpirationDate > DateTime.Now).ToList();
            switch (sortOrder)
            {
                case SortOrder.Asc:
                    return policies.OrderBy(p => p.Vehicle.Year).ToList();
                case SortOrder.Desc:
                    return policies.OrderByDescending(p => p.Vehicle.Year).ToList();
                default:
                    return policies;
            }

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
