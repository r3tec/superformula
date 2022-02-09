using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PolicyAPI.Data;
using PolicyAPI.Data.Models;
using System;
using Xunit;

namespace PolicyAPI.Test
{
    public class EndToEndTest : IClassFixture<Startup>
    {
        static readonly string[] Makes = { "Bummer", "Slummer", "Tummer" };
        static readonly string[] Models = { "Tinder", "Minder", "Slinder" };
        static readonly int[] Years = { 1201, 1590, 2222 };
        PolicyContext ctx = new PolicyContext();
        readonly PolicyService _service;

        public EndToEndTest(Startup fx)
        {
            _service = fx.ServiceProvider.GetService<PolicyService>();
        }

        [Fact]
        public void Can_add_delete()
        {
            var policy = new Data.Models.Policy() { EffectiveDate = DateTime.Now + TimeSpan.FromDays(50) };
            AddVehicles(policy, 5);
            ctx.Policies.Add(policy);
            ctx.SaveChanges();

            policy = ctx.Policies.FirstAsync().GetAwaiter().GetResult();
            Assert.Equal(5, policy.Vehicles.Count);
            ctx.Policies.Remove(policy);
            ctx.SaveChanges();
        }

        [Fact]
        public void Effective_date_30_days_in_The_future()
        {
            var policy = new Data.Models.Policy() { EffectiveDate = DateTime.Now + TimeSpan.FromDays(29) };
            AddVehicles(policy, 5);
            var exc = Assert.Throws<PolicyException>(() => _service.AddPolicy(policy));
            Assert.Equal(Reason.ThirtyDays, exc.ErrorCode);            
        }

        [Fact]
        public void Vehicle_year_before_1998()
        {
            var policy = new Data.Models.Policy() { EffectiveDate = DateTime.Now + TimeSpan.FromDays(50) };
            AddVehicles(policy, 1);
            policy.Vehicles[0].Year = 2000;
            var exc = Assert.Throws<PolicyException>(() => _service.AddPolicy(policy));
            Assert.Equal(Reason.Classic, exc.ErrorCode);
        }


        private void AddVehicles(Policy p, int cnt)
        {
            Random r = new Random();
            for (int i = 0; i < cnt; i++)
            {
                p.Vehicles.Add(new Vehicle()
                {
                    AttachedPolicy = p,
                    Manufacturer = Makes[r.Next(Makes.Length)],
                    Model = Models[r.Next(Models.Length)],
                    VehicleName = $"Car {i}",
                    Year = Years[r.Next(Years.Length)]
                });
            }
        }
    }
}
