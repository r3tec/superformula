using Microsoft.Extensions.Logging;
using PolicyAPI.Data.Models;
using PolicyAPI.Data.Repository;
using System;

namespace PolicyAPI.Data
{
    public class PolicyService
    {
        private readonly ILogger<PolicyService> _logger;
        private readonly IPolicyRepo _repo;
        public PolicyService(
            ILogger<PolicyService> logger,
            IPolicyRepo ctx
            )
        {
            _logger = logger;
            _repo = ctx;
        }

        public long AddPolicy(Policy p)
        {
            var period = p.EffectiveDate - DateTime.Now;
            if (period.TotalDays < 30)
                throw new PolicyException("Effective Date must be at least 30 days in the future from the creation date of the record") { ErrorCode = Reason.ThirtyDays };

            foreach(var v in p.Vehicles)
            {
                if (v.Year > 1998)
                    throw new PolicyException("Vehicle Year should be before 1998") { ErrorCode = Reason.Classic };
            }

            return _repo.Add(p);
        }
    }
}
