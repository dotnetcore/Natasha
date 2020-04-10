using Natasha.CSharpEngine;
using Natasha.CSharpEngine.Compile;
using Natasha.CSharpEngine.Log;
using Natasha.CSharpEngine.Syntax;
using Natasha.Error.Model;
using System;
using System.IO;
using System.Reflection;

namespace Natasha.CSharp
{

    public class AssemblyCSharpBuilder : NatashaCSharpEngine
    {

        /// <summary>
        /// 默认的输出文件夹
        /// </summary>
        public static string OutputFolder;
        static AssemblyCSharpBuilder()
        {
            OutputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DynamicLibraryFolders");
        }



        public bool CustomerUsingShut;
        public AssemblyCSharpBuilder() : this(Guid.NewGuid().ToString("N")){}
        public AssemblyCSharpBuilder(string name):base(name)
        {

            CustomerUsingShut = false;
            RetryLimit = 2;

        }




        public void CompilerOption(Action<NatashaCSharpCompiler> action)
        {
            action?.Invoke(Compiler);
        }
        public void SyntaxOptions(Action<NatashaCSharpSyntax> action)
        {
            action?.Invoke(Syntax);
        }




        public Assembly GetAssembly()
        {

            //如果是文件编译，则初始化路径
            if (Compiler.AssemblyOutputKind == Framework.AssemblyBuildKind.File)
            {

                if (Compiler.DllPath == default)
                {
                    Compiler.DllPath = Path.Combine(OutputFolder, Compiler.AssemblyName, ".dll");
                    Compiler.PdbPath = Path.Combine(OutputFolder, Compiler.AssemblyName, ".pdb");
                }

            }


            //进入编译流程
            Compile();


            //如果编译出错
            if (Compiler.Assembly == null && Exceptions != null && Exceptions.Count > 0 )
            {

                switch (Compiler.ErrorBehavior)
                {

                    case ExceptionBehavior.Log | ExceptionBehavior.Throw:
                        LogOperator.ErrorRecoder(Compiler.Compilation, Exceptions);
                        throw Exceptions[0];
                    case ExceptionBehavior.Log:
                        LogOperator.ErrorRecoder(Compiler.Compilation, Exceptions);
                        break;
                    case ExceptionBehavior.Throw:
                        throw Exceptions[0];
                    default:
                        break;

                }

            }
            else
            {

                LogOperator.SucceedRecoder(Compiler.Compilation);

            }
            return Compiler.Assembly;

        }



    }
}
