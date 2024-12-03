using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Runtime.CompilerServices;

namespace WebapiWIthController
{
    public class Program
    {
        public static void Main(string[] args)
        {

            NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var action = @"
var modelMetadataProvider = arg1.Services.GetService<IModelMetadataProvider>();
var controllerActivatorProvider = arg1.Services.GetService<IControllerPropertyActivator>();
((DefaultModelMetadataProvider)modelMetadataProvider).ClearCache();
((DefaultControllerPropertyActivator)controllerActivatorProvider).ClearCache();
Console.WriteLine(1111);"
                .WithMetadata(typeof(Console))
                .WithUsings("Microsoft.AspNetCore.Mvc.Controllers")
                .WithMetadata(typeof(IgnoresAccessChecksToAttribute))
                .WithMetadata(typeof(IModelMetadataProvider))
                .WithPrivateAccess(typeof(DefaultModelMetadataProvider))
                .ToAction<WebApplication>()!;
            action(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
