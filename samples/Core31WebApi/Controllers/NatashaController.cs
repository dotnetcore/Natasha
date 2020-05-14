using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Natasha;
using Natasha.CSharp;
using System;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core31WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NatashaController : Controller
    {
        public static Action SvcAction;
        public static Action EndPointAction;
        [HttpGet("{csharpCode}")]
        public string Get(string csharpCode,
            [FromServices] ApplicationPartManager manager,
            [FromServices] DynamicChangeTokenProvider tokenProvider)
        {
            
            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder();
            builder.Compiler.Domain = DomainManagement.Random;
            builder.Syntax.Add(HttpUtility.UrlDecode(csharpCode));
            manager.ApplicationParts.Add(new AssemblyPart(builder.GetAssembly()));
            tokenProvider.NotifyChanges();
            return "succeed!";

        }

       
    }
}
