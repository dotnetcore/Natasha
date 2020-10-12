using System.Text;

namespace Natasha.CSharp.Template
{

    public class PropertyTemplate<T> : DefinedNameTemplate<T> where T : PropertyTemplate<T>, new()
    {
        internal string _getter_body;
        internal string _setter_body;
        internal StringBuilder _getter;
        internal StringBuilder _setter;
        private bool _onlyGetter;
        private bool _onlySetter;

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

            _setter_body = body;
            return Link;

        }
        public T OnlySetter(string body)
        {
            _onlySetter = true;
            return Setter(body);
        }
        /// <summary>
        /// 直接传属性Set方法的内容字符串
        /// </summary>
        /// <param name="body">属性Set内容</param>
        /// <returns></returns>
        public T Getter(string body)
        {

            _getter_body = body;
            return Link;

        }
        public T OnlyGetter(string body)
        {
            _onlyGetter = true;
            return Getter(body);
        }




        public override T BuilderScript()
        {

            _getter.Clear();
            _setter.Clear();
            // [Attribute]
            // [access] [modifier] [type] [Name]{[{this}]}
            base.BuilderScript();
            if (!_onlySetter)
            {

                if (_getter_body == default)
                {

                    _getter.AppendLine("get;");

                }
                else
                {

                    _getter.Append("get{");
                    _getter.Append(_getter_body);
                    _getter.AppendLine("}");

                }

            }
            

            if (!_onlyGetter)
            {

                if (_setter_body == default)
                {

                    _setter.AppendLine("set;");

                }
                else
                {

                    _getter.Append("set{");
                    _getter.Append(_setter_body);
                    _setter.AppendLine("}");

                }

            }
            


            _script.AppendLine("{");
            _script.Append(_getter);
            _script.Append(_setter);
            _script.Append('}');
            return Link;

        }

    }

}
