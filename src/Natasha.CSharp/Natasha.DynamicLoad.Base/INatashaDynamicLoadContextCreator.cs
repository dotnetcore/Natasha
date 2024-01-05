using System.Collections.Generic;
using System.Reflection;
namespace Natasha.DynamicLoad.Base
{
    /// <summary>
    /// 域创建接口
    /// </summary>
    public interface INatashaDynamicLoadContextCreator
    {
        /// <summary>
        /// 返回满足 Natasha 编译标准的域
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        INatashaDynamicLoadContextBase CreateContext(string key);
        /// <summary>
        /// 返回满足 Natasha 编译标准的共享域
        /// </summary>
        /// <returns></returns>
        INatashaDynamicLoadContextBase CreateDefaultContext();
        /// <summary>
        /// 根据程序集获取域
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        INatashaDynamicLoadContextBase? GetDomain(Assembly assembly);
        /// <summary>
        /// 根据程序集获取元数据流
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="blob"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        unsafe bool TryGetRawMetadata(Assembly assembly, out byte* blob, out int length);
        /// <summary>
        /// 根据程序集获取该程序集所有的引用
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<Assembly>? GetDependencyAssemblies(Assembly assembly);
    }
}