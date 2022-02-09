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
        Policy GetById(int id);
        long Add(Policy p);
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

        public Policy GetById(int id)
        {
            return dbContext.Policies.Where(p => p.Id == id).FirstOrDefault();
        }

        public long Add(Policy p)
        {
            dbContext.Policies.Add(p);
            dbContext.SaveChanges();
            return p.Id;
        }
    }
}
