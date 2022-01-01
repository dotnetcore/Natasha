using Natasha.CSharp.Component.Domain;
using System.Reflection;


public static class AssemblyNameExtension
{
    public static string GetUniqueName(this AssemblyName assemblyName)
    {
        return string.IsNullOrEmpty(assemblyName.Name) ? assemblyName.FullName : assemblyName.Name;
    }
    public static LoadVersionResultEnum CompareWith(this AssemblyName oldName, AssemblyName newName, LoadBehaviorEnum loadBehavior)
    {
        if (loadBehavior == LoadBehaviorEnum.None)
        {
            return LoadVersionResultEnum.NoAction;
        }
        else if (loadBehavior == LoadBehaviorEnum.UseBeforeIfExist)
        {
            return LoadVersionResultEnum.UseBefore;
        }
        else if (oldName.Version != default && newName.Version != default)
        {
            if (loadBehavior == LoadBehaviorEnum.UseHighVersion)
            {
                if (oldName.Version < newName.Version)
                {
                    return LoadVersionResultEnum.UseAfter;
                }
            }
            else if (loadBehavior == LoadBehaviorEnum.UseLowVersion)
            {
                if (oldName.Version > newName.Version)
                {
                    return LoadVersionResultEnum.UseAfter;
                }
            }
            return LoadVersionResultEnum.UseBefore;
        }
        return LoadVersionResultEnum.NoAction;
    }
}

