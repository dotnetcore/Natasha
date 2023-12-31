using System.Reflection;

namespace Natasha.CSharp.Extension.Inner
{
    internal static class ReflectionInfoExtension
    {
        internal static MethodInfo GetMethodInfo(this PropertyInfo propertyInfo)
        {
            var methodInfo = propertyInfo.GetGetMethod(true);
            if (methodInfo == null)
            {
                methodInfo = propertyInfo.GetSetMethod(true);
            }
            return methodInfo!;
        }

        internal static MethodInfo GetMethodInfo(this EventInfo eventInfo)
        {
            var methodInfo = eventInfo.AddMethod;
            if (methodInfo == null)
            {
                methodInfo = eventInfo.RemoveMethod;
            }
            return methodInfo!;
        }
    }
}

