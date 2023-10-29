using System.Text;

namespace Natasha.CSharp.Template
{

    public class PropertyTemplate<T> : DefinedNameTemplate<T> where T : PropertyTemplate<T>, new()
    {
        internal string? _getter_body;
        internal string? _setter_body;
        internal readonly StringBuilder _getter;
        internal readonly StringBuilder _setter;
        private bool _onlyGetter;
        private bool _onlySetter;
        private string? _setterWordkey;


        public PropertyTemplate()
        {
            _setterWordkey = "set";
            _getter = new StringBuilder();
            _setter = new StringBuilder();
        }



        public T InitSetter(string? body = default)
        {
            _setterWordkey = "init";
            return Setter(body);
        }
        /// <summary>
        /// 直接传属性Set方法的内容字符串
        /// </summary>
        /// <param name="body">属性Set内容</param>
        /// <returns></returns>
        public T Setter(string? body = default)
        {
            _setter_body = body;
            return Link;

        }
        public T OnlySetter(string? body = default)
        {
            _onlySetter = true;
            return Setter(body);
        }
        /// <summary>
        /// 直接传属性Set方法的内容字符串
        /// </summary>
        /// <param name="body">属性Set内容</param>
        /// <returns></returns>
        public T Getter(string? body = default)
        {

            _getter_body = body;
            return Link;

        }
        public T OnlyGetter(string? body = default)
        {
            _onlyGetter = true;
            return Getter(body);
        }




        public override T BuilderScript()
        {

            _getter.Clear();
            _setter.Clear();
            // [comment]
            // [attribute]
            // [access] [modifier] [type] [name]{[{this}]}
            base.BuilderScript();
            if (!_onlySetter)
            {

                if (string.IsNullOrEmpty(_getter_body))
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

                if (string.IsNullOrEmpty(_setter_body))
                {

                    _setter.AppendLine($"{_setterWordkey};");

                }
                else
                {

                    _getter.Append($"{_setterWordkey}{{");
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
