using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Natasha;
using Natasha.CSharp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core31WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NatashaController : Controller
    {

        // GET api/<controller>/5
        [HttpGet("{code}")]
        public string Get(string code)
        {

            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder();
            builder.Compiler.Domain = DomainManagement.Default;
            builder.Syntax.Add(code);
            builder.GetAssembly();
            return "succeed!";

        }

       
    }
}
