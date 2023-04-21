using Natasha.CSharp.Component.Domain;
using System.Reflection;

namespace Natasha.Domain.Extension
{
    public static class AssemblyNameExtension
    {
        /// <summary>
        /// 获取程序集名对应的标识
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static string GetUniqueName(this AssemblyName assemblyName)
        {
            return string.IsNullOrEmpty(assemblyName.Name) ? assemblyName.FullName : assemblyName.Name;
        }

        /// <summary>
        /// 根据 loadBehavior 参数比较两个程序集版本
        /// </summary>
        /// <param name="customaryName">客服程序集名</param>
        /// <param name="defaultName">默认程序集名</param>
        /// <param name="loadBehavior">加载行为</param>
        /// <returns></returns>
        public static LoadVersionResultEnum CompareWithDefault(this AssemblyName customaryName, AssemblyName defaultName, LoadBehaviorEnum loadBehavior)
        {
            if (loadBehavior == LoadBehaviorEnum.None)
            {
                return LoadVersionResultEnum.NoAction;
            }
            else if (loadBehavior == LoadBehaviorEnum.UseDefault)
            {
                return LoadVersionResultEnum.UseDefault;
            }
            else if (loadBehavior == LoadBehaviorEnum.UseCustom)
            {
                return LoadVersionResultEnum.UseCustomer;
            }
            else if (customaryName.Version != default && defaultName.Version != default)
            {
                if (loadBehavior == LoadBehaviorEnum.UseHighVersion)
                {
                    if (customaryName.Version > defaultName.Version)
                    {
                        return LoadVersionResultEnum.UseCustomer;
                    }
                }
                else if (loadBehavior == LoadBehaviorEnum.UseLowVersion)
                {
                    if (customaryName.Version < defaultName.Version)
                    {
                        return LoadVersionResultEnum.UseCustomer;
                    }
                }
                return LoadVersionResultEnum.UseDefault;
            }
            return LoadVersionResultEnum.NoAction;
        }
    }
}

