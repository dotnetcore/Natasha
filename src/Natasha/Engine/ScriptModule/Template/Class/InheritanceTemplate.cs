using System;
using System.Text;

namespace Natasha
{
    public class InheritanceTemplate<T> : ClassNameTemplate<T>
    {
        public readonly StringBuilder Inheritances;

        public InheritanceTemplate()
        {
            Inheritances = new StringBuilder();
        }

        public T Inheritance(string type)
        {
            if (Inheritances.Length > 0)
            {
                Inheritances.Append($",{type}");
            }
            else
            {
                Inheritances.Append(":");
                Inheritances.Append(type);
            }
            return Link;
        }

        /// <summary>
        /// 设置继承
        /// </summary>
        /// <param name="types">类型</param>
        /// <returns></returns>
        public T Inheritance(params string[] types)
        {
            if (types != null && types.Length > 0)
            {
                for (int i = 1; i < types.Length; i++)
                {
                    Inheritance(types[i]);
                }
            }
            return Link;
        }
        public T Inheritance(params Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                Inheritance(types[i]);
            }
            return Link;
        }
        public T Inheritance<S>()
        {
            return Inheritance(typeof(S));
        }
        public T Inheritance(Type type)
        {
            if (type == null)
            {
                return Link;
            }
            UsingRecoder.Add(type);
            return Inheritance(type.GetDevelopName());
        }

        public override T Builder()
        {
            base.Builder();
            _script.Append($@"{Inheritances}{{");
            return Link;
        }
    }
}
