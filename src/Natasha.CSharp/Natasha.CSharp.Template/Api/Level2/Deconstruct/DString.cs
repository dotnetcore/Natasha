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
           out NatashaException? Exception)
        {

            AssemblyCSharpBuilder assembly = new();
            assembly.Add(script);
            Assembly = assembly.GetAssembly();
            if (assembly.Exceptions.Count>0)
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
