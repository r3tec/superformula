using Microsoft.Extensions.Logging;
using PolicyAPI.Data.Models;
using PolicyAPI.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyAPI.Data
{
    public interface IPolicyService
    {
        Task<Policy> AddPolicy(Policy p);
        void CheckStateRegulations(Policy p);
    }

    public class PolicyService : IPolicyService
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

        public async Task<Policy> AddPolicy(Policy p)
        {

            var period = p.EffectiveDate - DateTime.Now;
            if (period.TotalDays < 30)
                throw new PolicyException("Effective Date must be at least 30 days in the future from the creation date of the record") { ErrorCode = Reason.ThirtyDays };

            foreach (var v in p.Vehicles)
            {
                if (v.Year > 1998)
                    throw new PolicyException("Vehicle Year should be before 1998") { ErrorCode = Reason.Classic };
            }


            CheckAddress(p);

            CheckStateRegulations(p);
            var newPolicy = await _repo.Add(p);
            if (newPolicy != null)
                NotifyCreate(newPolicy);
            return newPolicy;
        }

        public void CheckAddress(Policy p)
        {
            if(string.IsNullOrWhiteSpace(p.Address) || !char.IsDigit(p.Address.ToCharArray()[0]))
                throw new PolicyException($"Incorrect address format: {p.Address}") { ErrorCode = Reason.AddressFormat };
        }
        public void CheckStateRegulations(Policy p)
        {
            Random r = new Random();
            switch (r.Next(3))
            {
                case 0:
                    throw new PolicyException("State Regulations do not allow this policy to be created") { ErrorCode = Reason.StateUnhappy };
                default:
                    return;
            }
        }

        private async void NotifyCreate(Policy p)
        {
            List<Task> notifyTasks = new List<Task>();

            foreach (var a in "one two three".Split(" "))
            {
                notifyTasks.Add(Task.Run(() => DoNotify(p, a)));
            }

            await Task.WhenAll(notifyTasks.ToArray());
        }

        private async Task DoNotify(Policy p, string caller)
        {
            bool result = new Random(DateTime.Now.Second).Next(2) > 0;
            string msg = $"Notification for caller {caller} " + (result ? "succeeded" : "failed");
            await Task.Delay(3000);
            _logger.LogDebug(msg);
            Console.WriteLine(msg);
        }
    }
}
