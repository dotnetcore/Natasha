using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha.AssemblyModule.Model
{
    public class AssemblyUnitInfo
    {

        public readonly Guid Id;
        public readonly LinkedListNode<PortableExecutableReference> Reference;
        public readonly Assembly Assembly;

        public AssemblyUnitInfo(AssemblyDomain context ,string path):this(context, new FileStream(path, FileMode.Open))
        {
           
        }

        public AssemblyUnitInfo(AssemblyDomain context, Stream stream)
        {

            stream.Position = 0;
            if (context.Name == "Default")
            {

                Assembly = AssemblyLoadContext.Default.LoadFromStream(stream);

            }
            else
            {
                Assembly = context.LoadFromStream(stream);
            }


            stream.Position = 0;
            Reference =new LinkedListNode<PortableExecutableReference>(MetadataReference.CreateFromStream(stream));
            Id = Assembly.ManifestModule.ModuleVersionId;
            stream.Dispose();

        }




        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
