using System.Reflection;
using System.Runtime.CompilerServices;

namespace Natasha.CSharp.Template.Reverser
{
    public static class AsyncReverser
    {

        public static string GetAsync(MethodInfo info)
        {

            return info.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) == null ? string.Empty : "async ";

        }

    }

}
