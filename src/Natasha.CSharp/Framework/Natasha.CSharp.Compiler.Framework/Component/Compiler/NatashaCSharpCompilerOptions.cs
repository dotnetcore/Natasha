using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Component.Compiler;
using Natasha.CSharp.Component.Compiler.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace Natasha.CSharp.Compiler
{
    public sealed class NatashaCSharpCompilerOptions
    {

        public NatashaCSharpCompilerOptions()
        {
            _reportDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>(GlobalSupperessCache._globalSuppressDiagnostics);
#if DEBUG
            this.WithCompilerFlag(CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes | CompilerBinderFlags.IgnoreAccessibility | CompilerBinderFlags.GenericConstraintsClause | CompilerBinderFlags.SuppressObsoleteChecks)
#else
            this.WithCompilerFlag(CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes | CompilerBinderFlags.GenericConstraintsClause | CompilerBinderFlags.SuppressObsoleteChecks)
#endif
            .WithNullableCompile(NullableContextOptions.Enable)
            .WithOutputKind(OutputKind.DynamicallyLinkedLibrary)
            .WithPlatform(Platform.AnyCpu);
        }

        private readonly ConcurrentDictionary<string, ReportDiagnostic> _reportDiagnostics;

        /// <summary>
        /// 屏蔽错误代码
        /// </summary>
        /// <param name="errorcode">错误代码，例如: CS0234</param>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions AddSupperess(string errorcode)
        {
            _reportDiagnostics[errorcode] = ReportDiagnostic.Suppress;
            return this;
        }

        /// <summary>
        /// 指定错误代码输出的报告级别,一旦发生此错误代码，将会按照报告级别进行报告。
        /// </summary>
        /// <param name="errorcode">错误代码例如：CS0234</param>
        /// <param name="reportLevel">报告级别例如：ReportDiagnostic.Error</param>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions SetCodeReportLevel(string errorcode, ReportDiagnostic reportLevel = ReportDiagnostic.Default)
        {
            _reportDiagnostics[errorcode] = reportLevel;
            return this;
        } 
        /// <summary>
        /// 移除自定义配置的错误代码
        /// </summary>
        /// <param name="errorcode"></param>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions RemoveSupperess(string errorcode)
        {
            _reportDiagnostics.Remove(errorcode);
            return this;
        }


        private bool _suppressReportShut = false;
        /// <summary>
        /// 启用禁断的报告
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithSuppressReportor()
        {
            _suppressReportShut = true;
            return this;
        }
        /// <summary>
        /// 关闭禁断的报告(默认)
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithoutSuppressReportor()
        {
            _suppressReportShut = false;
            return this;
        }

        /// <summary>
        /// 将诊断信息作为什么等级输出，如果重置了标记，比如 Error, 则所有诊断信息都将作为 Error 级别输出。
        /// 一旦你有 Diagnostic 产生，则编译失败。默认为： Default
        /// </summary>
        /// <param name="reportDiagnostic"></param>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithDiagnosticLevel(ReportDiagnostic reportDiagnostic = ReportDiagnostic.Default)
        {
            _reportLevel = reportDiagnostic;
            return this;
        }

        private ReportDiagnostic _reportLevel = ReportDiagnostic.Default;

        private NullableContextOptions _nullableCompileOption;
        /// <summary>
        /// 设置可空引用的检查级别,默认开启
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithNullableCompile(NullableContextOptions flag)
        {
            _nullableCompileOption = flag;
            return this;
        }


        private Platform _processorPlatform;
        /// <summary>
        /// 设置平台属性，默认 AnyCpu
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithPlatform(Platform flag)
        {
            _processorPlatform = flag;
            return this;
        }

        private OutputKind _assemblyKind;
        /// <summary>
        /// 设置输出类型，默认 OutputKind.DynamicallyLinkedLibrary
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithOutputKind(OutputKind flag)
        {
            _assemblyKind = flag;
            return this;
        }

        private bool _allowUnsafe;
        /// <summary>
        /// 允许使用非安全代码编译(默认)
        /// </summary>
        /// <returns></returns>
        public NatashaCSharpCompilerOptions WithUnsafeCompile()
        {
            _allowUnsafe = true;
            return this;
        }
        /// <summary>
        /// 不允许使用非安全代码编译
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
        public NatashaCSharpCompilerOptions WithCompilerFlag(CompilerBinderFlags flags)
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
        /// 导入公共元数据(默认)
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
        /// 导入全部元数据
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
        internal CSharpCompilationOptions GetCompilationOptions(OptimizationLevel optimizationLevel,bool withDebugInfo)
        {
            var compilationOptions = new CSharpCompilationOptions(
                                   //deterministic: true,
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
                                   generalDiagnosticOption: _reportLevel);
            if (_compileFlags != 0)
            {
                CompilerInnerHelper.SetTopLevelBinderFlagDelegate(compilationOptions, (uint)_compileFlags);
            }
            //CS1704
            CompilerInnerHelper.SetReferencesSupersedeLowerVersionsDelegate(compilationOptions, _referencesSupersedeLowerVersions);
            //Release with debugInfo (have a good il)
            CompilerInnerHelper.SetDebugPlusModeDelegate(compilationOptions, withDebugInfo);
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




