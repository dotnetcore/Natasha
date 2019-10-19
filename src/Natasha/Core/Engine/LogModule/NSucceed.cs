using Microsoft.CodeAnalysis.CSharp;
using Natasha.Log.Model;
using System;
using System.Reflection;

namespace Natasha.Log
{

    public class NSucceed : ALogWrite
    {

        public static bool Enabled;

        static NSucceed() => Enabled = true;




        public override void Write()
        {
            NWriter<NSucceed>.Recoder(Buffer);
        }




        public void Handler(CSharpCompilation compilation)
        {

            Buffer.AppendLine($"\r\n\r\n========================Succeed : {compilation.AssemblyName}========================\r\n");
            WrapperCode(compilation.SyntaxTrees);
            Buffer.AppendLine("\r\n\r\n-----------------------------------------------succeed------------------------------------------------");
            Buffer.AppendLine($"\r\n    Time :\t\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            Buffer.AppendLine($"\r\n    Lauguage :\t{compilation.Language} & {compilation.LanguageVersion}");
            Buffer.AppendLine($"\r\n    Target :\t{compilation.AssemblyName}");
            Buffer.AppendLine($"\r\n    Assembly : \t{compilation.AssemblyName}");
            Buffer.AppendLine("\r\n--------------------------------------------------------------------------------------------------------");
            Buffer.AppendLine("\r\n====================================================================");

        }

    }

}
