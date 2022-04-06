using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Component.Compiler;
using Natasha.CSharp.Component.Compiler.Utils;
using System;
using System.Collections.Concurrent;


namespace Natasha.CSharp.Compiler
{
    public sealed class NatashaCSharpCompilerOptions
    {

        public NatashaCSharpCompilerOptions()
        {
            _suppressDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>(GlobalSupperessCache._globalSuppressDiagnostics);
#if DEBUG
            this.SetCompilerFlag(CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes | CompilerBinderFlags.IgnoreAccessibility)
#else
        this.SetCompilerFlag(CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes)
#endif
        .SetNullableCompile(NullableContextOptions.Disable)
            .SetSupersedeLowerVersions(false)
            .SetOutputKind(OutputKind.DynamicallyLinkedLibrary)
            .CompileAsRelease()
            .SetUnsafeCompiler(true)
            .SetPlatform(Platform.AnyCpu);

        }

        private readonly ConcurrentDictionary<string, ReportDiagnostic> _suppressDiagnostics;

        public NatashaCSharpCompilerOptions AddSupperess(string errorcode)
        {
            _suppressDiagnostics[errorcode] = ReportDiagnostic.Suppress;
            return this;
        }

        public NatashaCSharpCompilerOptions RemoveSupperess(string errorcode)
        {
            _suppressDiagnostics.Remove(errorcode);
            return this;
        }


        private bool _suppressReportShut;
        public NatashaCSharpCompilerOptions UseSuppressReportor(bool shut)
        {
            _suppressReportShut = shut;
            return this;
        }


        private NullableContextOptions _nullableCompileOption;
        public NatashaCSharpCompilerOptions SetNullableCompile(NullableContextOptions flag)
        {
            _nullableCompileOption = flag;
            return this;
        }


        private OptimizationLevel _codeOptimizationLevel;
        public NatashaCSharpCompilerOptions CompileAsDebug()
        {
            _codeOptimizationLevel = OptimizationLevel.Debug;
            return this;
        }
        public NatashaCSharpCompilerOptions CompileAsRelease()
        {
            _codeOptimizationLevel = OptimizationLevel.Release;
            return this;
        }


        private Platform _processorPlatform;
        public NatashaCSharpCompilerOptions SetPlatform(Platform flag)
        {
            _processorPlatform = flag;
            return this;
        }

        private OutputKind _assemblyKind;
        public NatashaCSharpCompilerOptions SetOutputKind(OutputKind flag)
        {
            _assemblyKind = flag;
            return this;
        }

        private bool _allowUnsafe;
        public NatashaCSharpCompilerOptions SetUnsafeCompiler(bool shut)
        {
            _allowUnsafe = shut;
            return this;
        }


        private bool _referencesSupersedeLowerVersions;
        /// <summary>
        /// 自动禁用低版本程序集
        /// </summary>
        /// <param name="value"></param>
        public NatashaCSharpCompilerOptions SetSupersedeLowerVersions(bool shut)
        {
            _referencesSupersedeLowerVersions = shut;
            return this;
        }


        private CompilerBinderFlags _compileFlags;
        /// <summary>
        /// 绑定编译标识
        /// </summary>
        /// <param name="flags"></param>
        public NatashaCSharpCompilerOptions SetCompilerFlag(CompilerBinderFlags flags)
        {
            _compileFlags = flags;
            return this;
        }

        /// <summary>
        /// 移除 IgnoreAccessibility 标识
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions RemoveIgnoreAccessibility()
        {
            if (_compileFlags.HasFlag(CompilerBinderFlags.IgnoreAccessibility))
            {
                _compileFlags = (uint)_compileFlags - CompilerBinderFlags.IgnoreAccessibility;
            }
            return this;
        }

        /// <summary>
        /// 获取构建编译信息的选项
        /// </summary>
        /// <returns></returns>
        internal CSharpCompilationOptions GetCompilationOptions()
        {

            var compilationOptions = new CSharpCompilationOptions(
                                    nullableContextOptions: _nullableCompileOption,
                                   //strongNameProvider: a,
                                   deterministic: false,
                                   concurrentBuild: true,
                                   moduleName: Guid.NewGuid().ToString(),
                                   reportSuppressedDiagnostics: _suppressReportShut,
                                   metadataImportOptions: MetadataImportOptions.All,
                                   outputKind: _assemblyKind,
                                   optimizationLevel: _codeOptimizationLevel,
                                   allowUnsafe: _allowUnsafe,
                                   platform: _processorPlatform,
                                   checkOverflow: false,
                                   assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default,
                                   specificDiagnosticOptions: _suppressDiagnostics);
            if (_compileFlags != 0)
            {
                CompilerInnerHelper.SetTopLevelBinderFlagDelegate(compilationOptions, (uint)_compileFlags);
            }
            //CS1704
            CompilerInnerHelper.SetReferencesSupersedeLowerVersionsDelegate(compilationOptions, _referencesSupersedeLowerVersions);
            return compilationOptions;

        }


        ///// <summary>
        ///// 获取编译选项
        ///// </summary>
        ///// <param name="options"></param>
        ///// <returns></returns>
        //public CSharpCompilation GetCompilation()
        //{
        //    if (_compilation==default)
        //    {
        //        _compilation = CSharpCompilation.Create(AssemblyName, null, Domain.GetCompileReferences(), GetCompilationOptions());
        //    }
        //    return _compilation.RemoveAllSyntaxTrees();

        //}


    }
}




