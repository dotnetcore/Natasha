using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Natasha.CSharp;

namespace Core31Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        { 

            NatashaInitializer.InitializeAndPreheating();
            string result = NDelegate.RandomDomain().Func<string>("return \"a\";")();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
