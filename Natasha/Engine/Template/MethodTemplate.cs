using Natasha.Engine.Builder.Reverser;
using System.Reflection;
using System.Text;

namespace Natasha.Engine.Template
{
    public class MethodTemplate
    {
        private MethodInfo _info;
        private string _prefix;
        public StringBuilder _script;

        public MethodTemplate(MethodInfo info = null)
        {
            _info = info;
            _script = new StringBuilder();
        }

        public MethodTemplate New()
        {
            _prefix = "new";
            return this;
        }
        public MethodTemplate Override()
        {
            _prefix = "override";
            return this;
        }

        public MethodTemplate Virtual()
        {
            _prefix = "virtual";
            return this;
        }
        internal StringBuilder _text;
        /// <summary>
        /// 写内容
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public MethodTemplate Body(string text)
        {
            _text = new StringBuilder(text);
            return this;
        }
        public MethodTemplate Body(StringBuilder text)
        {
            _text = text;
            return this;
        }

        public string Builder()
        {
            if (_info.IsPublic)
            {
                _script.Append("public ");
            }
            else if (_info.IsPrivate)
            {
                _script.Append("private ");
            }
            if (_prefix != null)
            {
                _script.Append(_prefix);
                _script.Append(' ');
            }
            _script.Append(TypeReverser.Get(_info.ReturnType));
            _script.Append(' ');
            _script.Append(_info.Name);
            _script.Append("(");
            var parameters = _info.GetParameters();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    _script.Append(ParameterInfoReverser.Get(parameters[0]));
                    _script.Append(' ');
                    _script.Append(parameters[0].Name);
                    for (int i = 1; i < parameters.Length; i++)
                    {
                        _script.Append(',');
                        _script.Append(ParameterInfoReverser.Get(parameters[i]));
                        _script.Append(' ');
                        _script.Append(parameters[i].Name);
                    }
                }
            }
            _script.Append("){");
            _script.Append(_text);
            _script.Append("}");
            return _script.ToString();
        }
        public string Create(string content)
        {
            if (_info.IsPublic)
            {
                _script.Append("public ");
            }
            else if (_info.IsPrivate)
            {
                _script.Append("private ");
            }
            if (_prefix != null)
            {
                _script.Append(_prefix);
                _script.Append(' ');
            }
            _script.Append(TypeReverser.Get(_info.ReturnType));
            _script.Append(' ');
            _script.Append(_info.Name);
            _script.Append("(");
            var parameters = _info.GetParameters();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    _script.Append(ParameterInfoReverser.Get(parameters[0]));
                    _script.Append(' ');
                    _script.Append(parameters[0].Name);
                    for (int i = 1; i < parameters.Length; i++)
                    {
                        _script.Append(',');
                        _script.Append(ParameterInfoReverser.Get(parameters[i]));
                        _script.Append(' ');
                        _script.Append(parameters[i].Name);
                    }
                }
            }
            _script.Append("){");
            _script.Append(content);
            _script.Append("}");
            return _script.ToString();
        }
    }
}
