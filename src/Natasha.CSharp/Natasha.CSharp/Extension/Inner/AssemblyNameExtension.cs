using Natasha.CSharp.Component.Domain;
using System.Reflection;


internal static class AssemblyNameExtension
{
    /// <summary>
    /// 获取程序集名对应的标识
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    internal static string GetUniqueName(this AssemblyName assemblyName)
    {
        return string.IsNullOrEmpty(assemblyName.Name) ? assemblyName.FullName : assemblyName.Name;
    }

    /// <summary>
    /// 根据 loadBehavior 参数比较两个程序集版本
    /// </summary>
    /// <param name="beforeName">前一个程序集名</param>
    /// <param name="afterName">后一个程序集名</param>
    /// <param name="loadBehavior">加载行为</param>
    /// <returns></returns>
    internal static LoadVersionResultEnum CompareWith(this AssemblyName beforeName, AssemblyName afterName, LoadBehaviorEnum loadBehavior)
    {
        if (loadBehavior == LoadBehaviorEnum.None)
        {
            return LoadVersionResultEnum.NoAction;
        }
        else if (loadBehavior == LoadBehaviorEnum.UseBeforeIfExist)
        {
            return LoadVersionResultEnum.UseBefore;
        }
        else if (beforeName.Version != default && afterName.Version != default)
        {
            if (loadBehavior == LoadBehaviorEnum.UseHighVersion)
            {
                if (beforeName.Version < afterName.Version)
                {
                    return LoadVersionResultEnum.UseAfter;
                }
            }
            else if (loadBehavior == LoadBehaviorEnum.UseLowVersion)
            {
                if (beforeName.Version > afterName.Version)
                {
                    return LoadVersionResultEnum.UseAfter;
                }
            }
            return LoadVersionResultEnum.UseBefore;
        }
        return LoadVersionResultEnum.NoAction;
    }
}

