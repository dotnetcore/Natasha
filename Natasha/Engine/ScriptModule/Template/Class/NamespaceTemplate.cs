using System;
using System.Text;

namespace Natasha
{
    public class NamespaceTemplate<T>:UsingTemplate<T>
    {
        public string NamespaceScript;
        private bool _hiddenNamesapce;
        public NamespaceTemplate()
        {
            _hiddenNamesapce = false;
            NamespaceScript = "NatashaDynimacSpace";
        }
        public T HiddenNameSpace(bool shut = true)
        {
            _hiddenNamesapce = shut;
            return Link;
        }
        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespace">命名空间</param>
        /// <returns></returns>
        public T Namespace(string @namespace)
        {
            NamespaceScript =@namespace;
            return Link;
        }
        public T Namespace<S>()
        {
            return Namespace(typeof(S));
        }
        public T Namespace(Type type)
        {
            NamespaceScript = type.Namespace;
            return Link;
        }

        public override string Builder()
        {
            if (!_hiddenNamesapce)
            {
                StringBuilder temp = new StringBuilder();
                temp.Append($@"namespace {NamespaceScript}{{{Script}}}");
                Script = temp;
            }
            return base.Builder();
        }
    }
}
