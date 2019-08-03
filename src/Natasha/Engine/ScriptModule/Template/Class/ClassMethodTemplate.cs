using System;
using System.Text;

namespace Natasha.Template
{

    public class ClassMethodTemplate<T>:ClassFieldTemplate<T>
    {

        public readonly StringBuilder ClassMethodScript;

        public ClassMethodTemplate() => ClassMethodScript = new StringBuilder();




        public T CreateMethod(Action<MethodDelegateTemplate<T>> action)
        {
            var handler = new MethodDelegateTemplate<T>();
            action?.Invoke(handler);
            handler.Builder();
            ClassMethodScript.Append(handler._script);
            return Link;
        }




        public override T Builder()
        {
            base.Builder();
            _script.Append(ClassMethodScript);
            return Link;
        }

    }

}
