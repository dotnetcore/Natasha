using Natasha.CSharp;
using Natasha.Error;
using System.Reflection;

namespace System
{
    public static class DString
    {

        public static void Deconstruct(
           this string script,
           out Assembly Assembly,
           out CompilationException Exception)
        {

            AssemblyCSharpBuilder assembly = new AssemblyCSharpBuilder();
            assembly.Add(script);
            Assembly = assembly.GetAssembly();
            if (assembly.Exceptions != null)
            {

                Exception = assembly.Exceptions[0];

            }
            else
            {

                Exception = null;

            }
            
        }

    }

}
