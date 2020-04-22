using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Natasha.CSharp;

namespace Core20Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string result = NDelegate.RandomDomain().Func<string>("return \"a\";")();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
