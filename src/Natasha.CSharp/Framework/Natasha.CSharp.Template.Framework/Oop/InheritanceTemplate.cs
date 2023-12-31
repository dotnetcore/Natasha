using System;
using System.Text;

namespace Natasha.CSharp.Template
{
    public class InheritanceTemplate<T> : DefinedNameTemplate<T> where T : InheritanceTemplate<T>, new()
    {

        public readonly StringBuilder Inheritances;

        public InheritanceTemplate() => Inheritances = new StringBuilder();




        public T InheritanceAppend(string type)
        {

            if (Inheritances.Length > 0)
            {

                Inheritances.Append($",{type}");

            }
            else
            {

                Inheritances.Append(" : ");
                Inheritances.Append(type);

            }


            return Link;

        }




        /// <summary>
        /// 设置继承
        /// </summary>
        /// <param name="types">类型</param>
        /// <returns></returns>
        public T InheritanceAppend(params string[]? types)
        {

            if (types != null && types.Length > 0)
            {

                for (int i = 1; i < types.Length; i++)
                {

                    InheritanceAppend(types[i]);

                }

            }


            return Link;

        }




        public T InheritanceAppend(params Type[]? types)
        {

            if (types != null && types.Length > 0)
            {

                for (int i = 0; i < types.Length; i++)
                {

                    InheritanceAppend(types[i]);

                }

            }

            return Link;

        }




        public T InheritanceAppend<S>()
        {

            return InheritanceAppend(typeof(S));

        }




        public T InheritanceAppend(Type? type)
        {

            if (type == null || type == typeof(object))
            {
                return Link;
            }
            return InheritanceAppend(type.GetDevelopName());

        }




        public override T BuilderScript()
        {
            // [comment]
            // [attribute]
            // [access] [modifier] [name] [:{this}]
            base.BuilderScript();
            _script.Append(Inheritances);
            return Link;

        }

    }

}
