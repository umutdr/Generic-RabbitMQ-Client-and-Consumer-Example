using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace RMQ
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHostBuilder hostBuilder = AppHelper.CreateHostBuilder();
            IHost host = hostBuilder.Build();
            await host.RunAsync();
        }
    }
}
