using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
/// <summary>
/// 程序集编译构建器 - 引用选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{
    private Func<IEnumerable<MetadataReference>, IEnumerable<MetadataReference>>? _referencesFilter;
    private CombineReferenceBehavior _combineReferenceBehavior = CombineReferenceBehavior.UseCurrent;
    private readonly ReferenceConfiguration _referenceConfiguration = new();
    private readonly List<MetadataReference> _specifiedReferences;

    /// <summary>
    /// 该方法允许共享域参与编译.
    /// <list type="bullet">
    /// <item>
    /// <description>[共享域] 元数据 [参与] 编译.</description>
    /// </item>
    /// <item>
    /// <description>[当前域] 元数据 [参与] 编译.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// 注：若两个域不同，且存在相同名称元数据，默认优先使用主域的元数据.
    /// </remarks>
    /// <param name="action">配置同名元数据的解决策略</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithCombineReferences(Action<ReferenceConfiguration>? action = null)
    {
        action?.Invoke(_referenceConfiguration);
        _combineReferenceBehavior = CombineReferenceBehavior.CombineDefault;
        return this;
    }


    /// <summary>
    /// 配置编译元数据的合并行为.
    /// <list type="bullet">
    /// <item>
    /// <description>[共享域] 元数据 [不参与] 编译.</description>
    /// </item>
    /// <item>
    /// <description>[当前域] 元数据 [参与] 编译.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithCurrentReferences()
    {
        _combineReferenceBehavior = CombineReferenceBehavior.UseCurrent;
        return this;
    }

    /// <summary>
    /// 使用外部指定的元数据引用进行编译.
    /// <list type="bullet">
    /// <item>
    /// <description>[共享域] 元数据 [不参与] 编译.</description>
    /// </item>
    /// <item>
    /// <description>[当前域] 元数据 [不参与] 编译.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// 使用 ClearOutsideReferences 可以清除本次传递的元数据引用.
    /// </remarks>
    /// <param name="metadataReferences"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithSpecifiedReferences(IEnumerable<MetadataReference> metadataReferences)
    {
        lock (_specifiedReferences)
        {
            _specifiedReferences.AddRange(metadataReferences);
        }
        _combineReferenceBehavior = CombineReferenceBehavior.UseSpecified;
        return this;
    }

    /// <summary>
    /// 清除由 WithSpecifiedReferences 方法传入的元数据引用.
    /// </summary>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder ClearOutsideReferences()
    {
        lock (_specifiedReferences)
        {
            _specifiedReferences.Clear();
        }
        return this;
    }


    /// <summary>
    /// 配置元数据引用过滤策略.
    /// </summary>
    /// <param name="referencesFilter"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder SetReferencesFilter(Func<IEnumerable<MetadataReference>, IEnumerable<MetadataReference>>? referencesFilter)
    {
        _referencesFilter = referencesFilter;
        return this;
    }
}
