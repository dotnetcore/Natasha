using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Natasha.Framework.Domain
{
    public class FakerDomain : DomainBase
    {
        public FakerDomain(string name) : base(name) { }
        public override Assembly CompileStreamCallback(string dllFile, string pdbFile, Stream stream, string AssemblyName)
        {
            throw new NotImplementedException();
        }

        public override HashSet<PortableExecutableReference> GetCompileReferences()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Assembly> GetPluginAssemblies()
        {
            throw new NotImplementedException();
        }

        public override HashSet<string> GetReferenceElements()
        {
            throw new NotImplementedException();
        }

        public override Assembly LoadPlugin(string path, bool needLoadDependence = true, params string[] excludePaths)
        {
            throw new NotImplementedException();
        }

        public override void RemovePlugin(string path)
        {
            throw new NotImplementedException();
        }

        protected override Assembly? Default_Resolving(AssemblyLoadContext context, AssemblyName name)
        {
            throw new NotImplementedException();
        }

        protected override IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2)
        {
            throw new NotImplementedException();
        }
    }
}
