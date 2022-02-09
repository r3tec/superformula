using Microsoft.EntityFrameworkCore;
using PolicyAPI.Data;
using System;
using Xunit;

namespace PolicyAPI.Test
{
    public class EndToEndTest
    {
        [Fact]
        public void TestDbReadWrite()
        {
            var id = DateTime.Now.Ticks;
            var ctx = new PolicyContext();
            var policy = new Data.Models.Policy() { Id = id };
            ctx.Policies.Add(policy);
            ctx.SaveChanges();
            policy = ctx.Policies.FirstAsync().GetAwaiter().GetResult();
            ctx.Policies.Remove(policy);
            ctx.SaveChanges();
            Assert.Equal(id, policy.Id);
        }
    }
}
