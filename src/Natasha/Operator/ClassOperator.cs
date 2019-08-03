using Natasha.Complier;
using System;

namespace Natasha
{
    public static class ClassOperator
    {


        /// <summary>
        /// 根据类名获取类，前提类必须是成功编译过的
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type GetType(string name)
        {

            return AssemblyOperator.Loader(ScriptComplierEngine.ClassMapping[name])[name];

        }

    }

}
