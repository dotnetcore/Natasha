using Natasha.Complier;
using System;
using System.Linq;

namespace Natasha.Operator
{
    public static class TypeOperator
    {


        /// <summary>
        /// 根据类名获取类，前提类必须是成功编译过的
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type GetType(string name)
        {
            return ScriptComplierEngine.ClassMapping[name].GetTypes().First(item => item.Name == name);

        }

    }

}
