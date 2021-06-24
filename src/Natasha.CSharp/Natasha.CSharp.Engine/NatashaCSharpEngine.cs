using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharpEngine.Error;
using Natasha.Error;
using Natasha.Error.Model;
using Natasha.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;

namespace Natasha.CSharpEngine
{

    public class NatashaCSharpEngine
    {

        public SyntaxBase Syntax;
        public CompilerBase<CSharpCompilation,CSharpCompilationOptions> Compiler;
        public List<NatashaException> Exceptions;


        public string OutputFolder;
        public bool CustomUsingShut;
        public ExceptionBehavior CompileErrorBehavior;
        public ExceptionBehavior SyntaxErrorBehavior;

        /// <summary>
        /// 是否要编译到文件中
        /// </summary>
        public bool OutputToFile
        {

            get { return Compiler.AssemblyOutputKind == AssemblyBuildKind.File; }
            set { Compiler.AssemblyOutputKind = value ? AssemblyBuildKind.File : AssemblyBuildKind.Stream; }

        }


        /// <summary>
        /// 是否允许非安全编译
        /// </summary>
        public bool AllowUnsafe
        {

            get { return Compiler.AllowUnsafe; }
            set { Compiler.AllowUnsafe = true; }

        }


        /// <summary>
        /// 是否使用 Release 优化
        /// </summary>
        public bool UseRelease
        {

            get { return Compiler.Enum_OptimizationLevel == OptimizationLevel.Release; }
            set { Compiler.Enum_OptimizationLevel = value ? OptimizationLevel.Release : OptimizationLevel.Debug; }

        }


        /// <summary>
        /// 程序集输出类型，DLL/控制台/等等
        /// </summary>
        public OutputKind OutputKind
        {

            get { return Compiler.Enum_OutputKind; }
            set { Compiler.Enum_OutputKind = value; }

        }


        /// <summary>
        /// 当前域
        /// </summary>
        public DomainBase Domain
        {

            get { return Compiler.Domain; }
            set { Compiler.Domain = value; }

        }


        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName
        {
            get { return Compiler.AssemblyName; }
            set { Compiler.AssemblyName = value; }
        }


        /// <summary>
        /// 是否使用新版本程序集
        /// </summary>
        public bool UseNewVersionAssmebly
        {
            get { return Domain.UseNewVersionAssmebly; }
            set { Domain.UseNewVersionAssmebly = value; }
        }


        /// <summary>
        /// 编译容错次数
        /// </summary>
        public int RetryLimit { get; set; }


        /// <summary>
        /// 当前编译错误次数
        /// </summary>
        private int _retryCount;


        /// <summary>
        /// 是否开启容错模式
        /// </summary>
        public bool CanRetry { get; set; }


        /// <summary>
        /// 是否使用共享库引用
        /// </summary>
        public bool UseShareLibraries { get; set; }



        public NatashaCSharpEngine(string assemblyName)
        {

            Syntax = SyntaxComponent.GetSyntax();
            Compiler = CompilerComponent.GetCompiler();
            Compiler.AssemblyName = assemblyName;
            Compiler.CompileFailedHandler += NatashaEngine_CompileFailedHandler;

        }


        /// <summary>
        /// 流编译失败之后调用的方法
        /// </summary>
        /// <param name="stream">失败的流</param>
        /// <param name="diagnostics">编译出现的错误</param>
        /// <param name="compilation">编译信息</param>
        private Assembly NatashaEngine_CompileFailedHandler(Stream stream, ImmutableArray<Diagnostic> diagnostics, CSharpCompilation compilation)
        {

            if (CanRetryCompile(diagnostics))
            {
                return Compile();
            }
            return null;
        }


        /// <summary>
        /// 拿到异常之后进行后处理，如果处理后可以重编译，将再次编译
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        private bool CanRetryCompile(ImmutableArray<Diagnostic> errors)
        {

            if (CanRetry)
            {

                _retryCount += 1;
                if (_retryCount < RetryLimit)
                {
                   
                    return true;
                }
                
            }
            Exceptions = NatashaExceptionAnalyzer.GetCompileException(Compiler.AssemblyName, errors);
            return false;

        }


        /// <summary>
        /// 对语法树进行编译
        /// </summary>
        internal virtual Assembly Compile()
        {

            if (UseShareLibraries)
            {
                Compiler.Domain.UseShareLibraries();
            }
            Exceptions = null;
            var trees = Syntax.TreeCache.Values;
            Syntax.Clear();
            return Compiler.ComplieToAssembly(trees);

        }
    }
}
