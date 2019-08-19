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




        public void Handler(CSharpCompilation compilation, Assembly assembly)
        {

            Buffer.AppendLine("\r\n\r\n------------------------------------------succeed-------------------------------------------");
            Buffer.AppendLine($"\r\n    Time :\t\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            Buffer.AppendLine($"\r\n    Lauguage :\t{compilation.Language} & {compilation.LanguageVersion}");
            Buffer.AppendLine($"\r\n    Target :\t\t{compilation.AssemblyName}");
            Buffer.AppendLine($"\r\n    Path :\t\t{assembly.Location}");
            Buffer.AppendLine($"\r\n    Assembly : \t{assembly.FullName}");
            Buffer.AppendLine("\r\n----------------------------------------------------------------------------------------------");
            WrapperTitle("Succeed : " + compilation.AssemblyName);

        }

    }

}
