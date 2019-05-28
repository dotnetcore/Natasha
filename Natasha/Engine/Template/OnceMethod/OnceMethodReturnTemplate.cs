using Natasha.Engine.Builder.Reverser;
using System;
using System.Reflection;

namespace Natasha
{
    public class OnceMethodReturnTemplate<T>: OnceModifierTemplate<T>
    {
        public string ReturnScript;
        public Type ReturnType;
        /// <summary>
        /// 设置返回类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns></returns>

        public T Return<S>()
        {
            return Return(typeof(S));
        }
        public T Return(MethodInfo info)
        {
            return Return(info.ReturnType);
        }
        public T Return(Type type = null)
        {
            if (type == null)
            {
                type = typeof(void);
            }
            ReturnType = type;
            UsingRecoder.Add(type);
            ReturnScript = NameReverser.GetName(type)+" ";
            return Link;
        }

        public override string Builder()
        {
            Script.Insert(0, ReturnScript);
            return base.Builder();
        }

    }
}
