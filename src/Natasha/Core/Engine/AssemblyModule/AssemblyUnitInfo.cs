using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha.Complier
{
    public class AssemblyUnitInfo
    {

        public readonly Guid Id;
        public readonly LinkedListNode<PortableExecutableReference> Reference;
        public readonly Assembly Assembly;


        public AssemblyUnitInfo(AssemblyDomain context, string path)
        {

            if (context.Name == "Default")
            {

                Assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);

            }
            else
            {

                Assembly = context.LoadFromAssemblyPath(path);

            }


            Reference = new LinkedListNode<PortableExecutableReference>(MetadataReference.CreateFromFile(path));
            Id = Assembly.ManifestModule.ModuleVersionId;

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
            Reference = new LinkedListNode<PortableExecutableReference>(MetadataReference.CreateFromStream(stream));
            Id = Assembly.ManifestModule.ModuleVersionId;
            stream.Dispose();

        }




        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
