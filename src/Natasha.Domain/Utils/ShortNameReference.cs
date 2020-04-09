using Microsoft.CodeAnalysis;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace Natasha.Core
{

    public class ShortNameReference : IDisposable
    {


        public readonly ConcurrentDictionary<string, PortableExecutableReference> References;
        public ShortNameReference()
        {
            References = new ConcurrentDictionary<string, PortableExecutableReference>();
        }




        public void AddNoRepeate(string path, ShortNameReference source)
        {

            if (!source.HasValue(path))
            {

                Add(path);

            }

        }




        public PortableExecutableReference Add(string path)
        {

            var reference = MetadataReference.CreateFromFile(path);
            if (reference!=default)
            {
                References[Path.GetFileName(path)] = reference;
            }
            return reference;

        }
        



        public bool HasValue(string path)
        {

            return References.ContainsKey(Path.GetFileName(path));

        }




        public void Remove(string path)
        {

            var shortName = Path.GetFileName(path);
            if (References.ContainsKey(shortName))
            {

                while(!References.TryRemove(shortName,out _)){ }

            }

        }





        public void Dispose()
        {

            References.Clear();

        }
    }

}
