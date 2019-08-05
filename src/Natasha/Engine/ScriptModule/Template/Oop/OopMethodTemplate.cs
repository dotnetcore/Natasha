using System;
using System.Text;

namespace Natasha.Template
{

    public class OopMethodTemplate<T>:OopFieldTemplate<T>
    {

        public readonly StringBuilder OopMethodScript;

        public OopMethodTemplate() => OopMethodScript = new StringBuilder();




        public T CreateMethod(Action<MethodDelegateTemplate<T>> action)
        {
            var handler = new MethodDelegateTemplate<T>();
            action?.Invoke(handler);
            handler.Builder();
            OopMethodScript.Append(handler._script);
            return Link;
        }




        public override T Builder()
        {
            base.Builder();
            _script.Append(OopMethodScript);
            return Link;
        }

    }

}
