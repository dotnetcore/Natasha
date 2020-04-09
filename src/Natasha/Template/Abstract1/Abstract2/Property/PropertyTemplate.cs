using System.Text;

namespace Natasha.Template
{

    public class PropertyTemplate<T> : DefinedNameTemplate<T> where T : PropertyTemplate<T>, new()
    {

        internal StringBuilder _getter;
        internal StringBuilder _setter;

        public PropertyTemplate()
        {
            _getter = new StringBuilder();
            _setter = new StringBuilder();
        }




        /// <summary>
        /// 直接传属性Set方法的内容字符串
        /// </summary>
        /// <param name="body">属性Set内容</param>
        /// <returns></returns>
        public T Setter(string body)
        {

            _setter.Append(body);
            return Link;

        }
        /// <summary>
        /// 直接传属性Set方法的内容字符串
        /// </summary>
        /// <param name="body">属性Set内容</param>
        /// <returns></returns>
        public T Getter(string body)
        {

            _getter.Append(body);
            return Link;

        }




        public override T BuilderScript()
        {

            // [Attribute]
            // [access] [modifier] [type] [Name]{[{this}]}
            base.BuilderScript();
            if (_getter.Length == 0)
            {

                _getter.AppendLine("get;");

            }
            else
            {

                _getter.Insert(0, "get{");
                _getter.AppendLine("}");

            }


            if (_setter.Length == 0)
            {

                _setter.AppendLine("set;");

            }
            else
            {

                _setter.Insert(0, "set{");
                _setter.AppendLine("}");

            }


            _script.AppendLine("{");
            _script.Append(_getter);
            _script.Append(_setter);
            _script.Append('}');
            return Link;

        }

    }

}
