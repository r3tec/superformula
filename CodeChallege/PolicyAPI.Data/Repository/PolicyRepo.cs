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
            dbContext.Policies.Find(1);
            return new Policy() { Id = 1 };
        }
    }
}
