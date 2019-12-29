using System.Reflection;
using System.Runtime.Loader;

namespace Natasha
{
    public static  class AssemblyExtension
    {

        /// <summary>
        /// 创建一个程序集编译类
        /// </summary>
        /// <param name="name">程序集名字</param>
        /// <returns></returns>
        public static NAssembly CreateAssembly(this AssemblyDomain domain, string name = default)
        {
            NAssembly result = new NAssembly(name);
            result.Options.Domain = domain;
            return result;
        }




        public static AssemblyDomain GetDomain(this Assembly assembly)
        {

            return (AssemblyDomain)AssemblyLoadContext.GetLoadContext(assembly);

        }




        public static void RemoveReferences(this Assembly assembly)
        {

            GetDomain(assembly).RemoveAssembly(assembly);

        }



        public static void DisposeDomain(this Assembly assembly)
        {

            GetDomain(assembly).Dispose();

        }

    }
}
