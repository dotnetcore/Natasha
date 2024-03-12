using Natasha.CSharp.Component.Load;

namespace System.Reflection
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
        public static AssemblyLoadVersionResult CompareWithDefault(this AssemblyName customaryName, AssemblyName defaultName, AssemblyCompareInformation loadBehavior)
        {
            if (loadBehavior == AssemblyCompareInformation.None)
            {
                return AssemblyLoadVersionResult.NoAction;
            }
            else if (loadBehavior == AssemblyCompareInformation.UseDefault)
            {
                return AssemblyLoadVersionResult.UseDefault;
            }
            else if (loadBehavior == AssemblyCompareInformation.UseCustom)
            {
                return AssemblyLoadVersionResult.UseCustomer;
            }
            else if (customaryName.Version != default && defaultName.Version != default)
            {
                if (loadBehavior == AssemblyCompareInformation.UseHighVersion)
                {
                    if (customaryName.Version > defaultName.Version)
                    {
                        return AssemblyLoadVersionResult.UseCustomer;
                    }
                }
                else if (loadBehavior == AssemblyCompareInformation.UseLowVersion)
                {
                    if (customaryName.Version < defaultName.Version)
                    {
                        return AssemblyLoadVersionResult.UseCustomer;
                    }
                }
                return AssemblyLoadVersionResult.UseDefault;
            }
            return AssemblyLoadVersionResult.NoAction;
        }
    }
}

