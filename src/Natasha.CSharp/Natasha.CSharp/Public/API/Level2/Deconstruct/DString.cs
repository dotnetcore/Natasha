using System.Reflection;

namespace System
{
    public static class DString
    {

        public static void Deconstruct(
           this string script,
           out Assembly? assembly,
           out NatashaCompilationLog log)
        {

            AssemblyCSharpBuilder builder = new();
            builder.Add(script);
            NatashaCompilationLog nlog = default!;
            builder.LogCompilationEvent += (item) => { nlog = item; };
            try
            {
                assembly = builder.GetAssembly();
                log = nlog;
            }
            catch (Exception ex)
            {
                log = nlog;
                throw;
            }
            


        }

    }

}
