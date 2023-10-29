using System;


public static class NatashaTypeExtension
{

    public static Delegate GetDelegateFromType(this Type type, string methodName, Type delegateType, object? target = null)
    {
        var info = type.GetMethod(methodName);
        try
        {
            if (info == null)
            {
                throw new Exception($"未从{type.FullName}中反射出{methodName}方法!");
            }
            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            NatashaException exception = new($"类型为 {type.FullName} 的 {methodName} 方法无法转成委托 {delegateType.Name}！错误信息:{ex.Message}")
            {
                ErrorKind = NatashaExceptionKind.Delegate
            };
            throw exception;

        }

    }
    public static T GetDelegateFromType<T>(this Type type, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromType(type, methodName, typeof(T), target);
    }

}

