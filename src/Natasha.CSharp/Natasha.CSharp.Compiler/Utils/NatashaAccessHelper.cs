using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace Natasha.CSharp.Compiler.Utils
{
    public static class NatashaAccessHelper
    {
        public readonly static Action<AssemblyCSharpBuilder> AccessHandle;

        static NatashaAccessHelper()
        {
            //            var accessType = Assembly.GetEntryAssembly().GetType("System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute", throwOnError: false);
            //            if (accessType == null)
            //            {
            //                AssemblyCSharpBuilder builder = new();
            //                builder.UseDefaultLoadContext();
            //                builder.UseSimpleMode();
            //                builder.ConfigLoadContext(opt => opt
            //                    .AddReferenceAndUsingCode(typeof(object))
            //                    .AddReferenceAndUsingCode(typeof(AssemblyName))
            //                );

            //                builder.Add(@"
            //namespace System.Runtime.CompilerServices
            //{
            //    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
            //    public class IgnoresAccessChecksToAttribute : Attribute
            //    {
            //        public IgnoresAccessChecksToAttribute(string assemblyName)
            //        {
            //            AssemblyName = assemblyName;
            //        }

            //        public string AssemblyName { get; }
            //    }
            //}");
            //                var assembly = builder.GetAssembly();
            //                accessType = assembly.GetTypeFromShortName("IgnoresAccessChecksToAttribute");
            //           }

            AccessHandle = builder =>
              builder
              .ConfigCompilerOption(opt => opt
                .WithAllMetadata()
                .AppendCompilerFlag(CompilerBinderFlags.IgnoreAccessibility)
               );
              //.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode(accessType));

        }
    }
}
