using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Natasha.CSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core31WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NatashaController : Controller
    {
        private readonly IComponentContext _icoContext;
        public static IServiceCollection services;
        public static ICollection<EndpointDataSource> Endpoints;
        public static ApplicationBuilder Builder;
        public static ContainerBuilder AutoFacBuilder;
        public static readonly ConcurrentQueue<AssemblyPart> AssemblyCache;
        static NatashaController()
        {
            AssemblyCache = new ConcurrentQueue<AssemblyPart>();
        }
        public NatashaController(IComponentContext component)
        {
            _icoContext = component;
        }
        [HttpPost]
        public string Post(string csharpCode,
            [FromServices] ApplicationPartManager manager,
            [FromServices] DynamicChangeTokenProvider tokenProvider)
        {
            string code = HttpUtility.UrlDecode(csharpCode);
            string name = ScriptHelper.GetClassName(code);
            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder("TestAAA");
            builder.Compiler.Domain = DomainManagement.Create(name);
            builder.Add(code);
            var assembly = builder.GetAssembly();
            var assemblyPart = new AssemblyPart(assembly);
            AssemblyCache.Enqueue(assemblyPart);
            manager.ApplicationParts.Add(assemblyPart);
            tokenProvider.NotifyChanges();
            //AutoFac.RegisterType(assembly.GetTypes().First(item=>item.Name=="TestController")).SingleInstance();
            //icoContext.Resolve(assembly.GetTypes().First(item => item.Name == "TestController"));
            
            AutoFacBuilder.RegisterAssemblyModules(assembly);
            var controllerType = assembly.GetTypes().First(item => item.Name == "TestController");
            AutoFacBuilder.RegisterType(controllerType).AsSelf().InstancePerLifetimeScope();
            _icoContext.ResolveOptional(controllerType);
            _icoContext.Resolve(controllerType);

            return controllerType.ToString();

        }

        [HttpDelete("{route}")]
        public string Delete(string route, 
            
            [FromServices] ApplicationPartManager manager,
            [FromServices] DynamicChangeTokenProvider tokenProvider)
        {

            var dataSource = ((IEndpointRouteBuilder)(Builder.Properties["__EndpointRouteBuilder"])).DataSources;
            EndpointDataSource cache = null;
            Endpoint endpoint = null; 
            foreach (var items in dataSource)
            {
                foreach (var item in items.Endpoints)
                {
                    if (item.DisplayName.Contains(route))
                    {
                        cache = items;
                        endpoint = item;
                    }
                }
            }
            if (endpoint!=null)
            {
                Endpoints.Remove(cache);
            }
            //Builder.Properties["a"]
            AssemblyPart assembly = null;
            foreach (var items in AssemblyCache)
            {
                var types = items.Assembly.ExportedTypes;
                foreach (var item in types)
                {
                    if (item.Name.Contains(route))
                    {
                        assembly = items;
                    }
                } 
            }
           
            if (assembly!=null)
            {

                AssemblyCache.TryDequeue(out assembly);
                assembly.Assembly.DisposeDomain();
                manager.ApplicationParts.Remove(assembly);
                tokenProvider.NotifyChanges();
                return "Succeed!";

            }
            
            return "No Such Type!";
        }


        [HttpGet("{typeName}")]
        public string Collection(string typeName)
        {

            for (int i = 0; i < 20; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            if (DomainManagement.IsDeleted(typeName))
            {
                return "Has Collection!";
            }
            else
            {
                
                var a = DomainManagement.Get(typeName);
                a.Dispose();
            }
            
            return "No Collection!";

        }
    }
}
