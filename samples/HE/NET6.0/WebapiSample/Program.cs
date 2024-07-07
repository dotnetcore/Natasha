using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Natasha.CSharp.HotExecutor.Component;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace WebapiSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //HE:Async
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            TestHECommand();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            var app = builder.Build();
            
            for (int i = 0; i < 7; i++)
            {
                GC.Collect();
            }
            foreach (var item in DomainManagement.Cache)
            {
                if (item.Value.Target != null)
                {
                    Console.WriteLine(item.Key + " alive!");
                }
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.AsyncToHotExecutor();
            app.MapControllers();
            var modelMetadataProvider = app.Services.GetService<IModelMetadataProvider>();
            var controllerActivatorProvider = app.Services.GetService<IControllerActivatorProvider>();
            var endBuilder = app.Services.GetService<IEndpointRouteBuilder>();
            var routeBuilder = app.Services.GetService<IRouteBuilder>();
            var swagger = app.Services.GetService<ISwaggerProvider>();

            HEProxy.SetPreHotExecut(() => {

                //endBuilder.DataSources.Clear();
                swagger.GetSwagger("v1").Paths.Clear();
                var methodInfo1 = modelMetadataProvider.GetType().GetMethod("ClearCache", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (methodInfo1 != null)
                {
                    methodInfo1?.Invoke(modelMetadataProvider, new object[0]);
                }
                //DS "In HEProxy.SetPreHotExecut2"
                var methodInfo2 = controllerActivatorProvider.GetType().GetMethod("ClearCache", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (methodInfo2 != null)
                {
                    methodInfo2?.Invoke(controllerActivatorProvider, new object[0]);
                }
            });
            app.Run();
        }

        public static void TestHECommand()
        {
            TestLocalMethod();
            static void TestLocalMethod()
            {
                //DS "In TestLocalMethod"
                Action lambda = () =>
                {
                    //DS "In lambda action"
                    InnerLocalMethod();
                    static void InnerLocalMethod()
                    {
                        Action lambda2 = () =>
                        {


                            //DS "In lambda2"
                            var temp = 6;
                            //DS temp+1
                            //DS temp+1 == 1
                            //DS "Out lambda2"
                        };
                        //DS "In InnerLocalMethod"
                        lambda2();
                        //DS "Out InnerLocalMethod"

                    }
                    //DS "Out lambda action"
                };
                lambda();
                //DS "Out TestLocalMethod"
            }
        }
    }
}
