using Microsoft.CodeAnalysis.CSharp;
using Natasha.Log;
using Natasha.Log.Model;

namespace System
{

    public class NSucceedLog : NatashaLogBase
    {

        public static bool Enabled;

        static NSucceedLog() => Enabled = false;




        public override void Write()
        {
            NatashaWriterIniter<NSucceedLog>.Recoder(Buffer);
        }




        public void Handler(CSharpCompilation compilation)
        {

            Buffer.AppendLine($"\r\n\r\n========================Succeed : {compilation.AssemblyName}========================\r\n");
            AddLineNumber(compilation.SyntaxTrees);
            Buffer.AppendLine("\r\n\r\n-----------------------------------------------succeed------------------------------------------------");
            Buffer.AppendLine($"\r\n    Time     :\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            Buffer.AppendLine($"\r\n    Lauguage :\t{compilation.Language} & {compilation.LanguageVersion}");
            Buffer.AppendLine($"\r\n    Target   :\t{compilation.AssemblyName}");
            Buffer.AppendLine($"\r\n    Assembly :\t{compilation.AssemblyName}");
            Buffer.AppendLine("\r\n--------------------------------------------------------------------------------------------------------");
            Buffer.AppendLine("\r\n====================================================================");

        }

    }

}
