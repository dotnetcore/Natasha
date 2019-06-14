using System;
using System.Linq;
using System.Reflection;

namespace Natasha
{
    public class AssemblyOperator
    {


        private readonly Assembly _assembly;
        public AssemblyOperator(Assembly assembly) => _assembly = assembly;




        /// <summary>
        /// 静态加载程序集
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static AssemblyOperator Loader(Assembly assembly)
        {
            return new AssemblyOperator(assembly);
        }




        /// <summary>
        /// 根据索引值获取程序集中的类
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public Type this[string className]
        {
            get { return _assembly.GetTypes().First(item=>item.Name==className); }
        }
    }
}
