using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Component.Compiler;
using Natasha.CSharp.Component.Compiler.Utils;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;


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
            .SetNullableCompile(NullableContextOptions.Enable)
            .WithoutLowerVersionsAssembly()
            .SetOutputKind(OutputKind.DynamicallyLinkedLibrary)
            .WithUnsafeCompile()
            .WithPublicMetadata()
            .SetPlatform(Platform.AnyCpu);

        }

        private readonly ConcurrentDictionary<string, ReportDiagnostic> _suppressDiagnostics;

        public NatashaCSharpCompilerOptions AddSupperess(string errorcode)
        {
            _suppressDiagnostics[errorcode] = ReportDiagnostic.Suppress;
            return this;
        }
        public NatashaCSharpCompilerOptions IgnoreWarning(string errorcode)
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
        /// <summary>
        /// 启用诊断报告
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithSuppressReportor()
        {
            _suppressReportShut = true;
            return this;
        }
        /// <summary>
        /// 禁用诊断报告(默认)
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithoutSuppressReportor()
        {
            _suppressReportShut = false;
            return this;
        }



        private NullableContextOptions _nullableCompileOption;
        public NatashaCSharpCompilerOptions SetNullableCompile(NullableContextOptions flag)
        {
            _nullableCompileOption = flag;
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
        /// <summary>
        /// 允许使用非安全代码编译
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithUnsafeCompile()
        {
            _allowUnsafe = true;
            return this;
        }
        /// <summary>
        /// 不允许使用非安全代码编译(默认)
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithoutUnsafeCompile()
        {
            _allowUnsafe = false;
            return this;
        }


        private bool _referencesSupersedeLowerVersions;
        /// <summary>
        /// 不禁用低版本程序集
        /// </summary>
        public NatashaCSharpCompilerOptions WithLowerVersionsAssembly()
        {
            _referencesSupersedeLowerVersions = true;
            return this;
        }
        /// <summary>
        /// 禁用低版本程序集(默认)
        /// </summary>
        public NatashaCSharpCompilerOptions WithoutLowerVersionsAssembly()
                {
            _referencesSupersedeLowerVersions = false;
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
        /// 增加编译标识
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions AppendCompilerFlag(params CompilerBinderFlags[] flags)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                _compileFlags |= flags[i];
            }
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

        private MetadataImportOptions _metadataImportOptions;
        /// <summary>
        /// 导入公共元数据
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithPublicMetadata() {
            _metadataImportOptions = MetadataImportOptions.Public;
            return this;
        }
        /// <summary>
        /// 导入内部元数据
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithInternalMetadata()
        {
            _metadataImportOptions = MetadataImportOptions.Internal;
            return this;
        }
        /// <summary>
        /// 导入全部元数据(默认)
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithAllMetadata()
        {
            _metadataImportOptions = MetadataImportOptions.All;
            return this;
        }
        /// <summary>
        /// 获取构建编译信息的选项
        /// </summary>
        /// <returns></returns>
        internal CSharpCompilationOptions GetCompilationOptions(OptimizationLevel optimizationLevel)
        {
            var compilationOptions = new CSharpCompilationOptions(
                                   nullableContextOptions: _nullableCompileOption,
                                   //strongNameProvider: a,
                                   deterministic: false,
                                   concurrentBuild: true,
                                   moduleName: Guid.NewGuid().ToString(),
                                   reportSuppressedDiagnostics: _suppressReportShut,
                                   metadataImportOptions: _metadataImportOptions,
                                   outputKind: _assemblyKind,
                                   optimizationLevel: optimizationLevel,
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




