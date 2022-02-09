using Microsoft.Extensions.DependencyInjection;
using PolicyAPI.Data;
using PolicyAPI.Data.Repository;

namespace PolicyAPI.Test
{
    public class Startup
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public Startup()
        {
            var services = new ServiceCollection();
            services.AddTransient<IPolicyRepo, PolicyRepo>();
            services.AddTransient<PolicyService>();
            services.AddTransient<PolicyContext>();
            ServiceProvider = services.AddLogging().BuildServiceProvider();
        }
    }
}
