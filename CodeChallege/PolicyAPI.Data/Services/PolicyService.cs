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
        Task<Policy> GetPolicy(long id, string license);
        Task<List<Policy>> GetAll(string license, SortOrder sortOrder, bool returnExpired);
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

        public async Task<List<Policy>> GetAll(string license, SortOrder sortOrder, bool returnExpired)
        {
            return await _repo.GetAll(license, sortOrder, returnExpired);
        }

        public async Task<Policy> GetPolicy(long id, string license)
        {
            return await _repo.GetById(id, license);
        }

        public async Task<Policy> AddPolicy(Policy p)
        {
            ValidatePolicy(p);
            var newPolicy = await _repo.Add(p);
            if (newPolicy != null)
                NotifyCreate(newPolicy);
            return newPolicy;
        }

        private void CheckAddress(Policy p)
        {
            if(string.IsNullOrWhiteSpace(p.Address) || !char.IsDigit(p.Address.ToCharArray()[0]))
                throw new PolicyException($"Incorrect address format: {p.Address}") { ErrorCode = Reason.AddressFormat };
        }
        private bool CheckStateRegulations(Policy p, out string error)
        {
            error = "";
            Random r = new Random();
            switch (r.Next(3))
            {
                case 0:
                    error = "State Regulations do not allow this policy to be created";
                    return false;
                default:
                    return true;
            }
        }

        private void ValidatePolicy(Policy p)
        {
            var period = p.EffectiveDate - DateTime.Now;
            if (period.TotalDays < 30)
                throw new PolicyException("Effective Date must be at least 30 days in the future from the creation date of the record") { ErrorCode = Reason.ThirtyDays };

            if (p.Vehicle.Year > 1998)
                throw new PolicyException("Vehicle Year should be before 1998") { ErrorCode = Reason.Classic };
            // some reasonable validation for effecftive date
            if ((p.ExpirationDate - p.EffectiveDate).TotalDays < 30)
                throw new PolicyException("Expiration Date must be at least 30 days after effective date") { ErrorCode = Reason.ExpireDate };
            // premium
            if (p.Premium < 1.0 || p.Premium > 100000.0)
                throw new PolicyException($"Policy premium value {p.Premium} is not valid");

            CheckAddress(p);

            if (!CheckStateRegulations(p, out string error))
                throw new PolicyException(error) { ErrorCode = Reason.StateUnhappy };
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
            for (int i = 1; i < 4; i++)
            {
                bool result = new Random(DateTime.Now.Second).Next(2) > 0;
                string msg = $"Notification for caller {caller} " + (result ? "succeeded" : "failed");
                _logger.LogDebug(msg);
                Console.WriteLine(msg);
                if (result)
                    break;
                await Task.Delay(3000);
            }
        }
    }
}
