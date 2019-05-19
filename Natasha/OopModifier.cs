using Natasha.Operator;
using Natasha.Standard;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class OopModifier<T> : OopModifier
    {
        internal static ConcurrentDictionary<string, Func<T>> _ctor_mapping;
        
        static OopModifier()
        {
            _ctor_mapping = new ConcurrentDictionary<string, Func<T>>();
        }

        public OopModifier() : base(typeof(T))
        {

        }

        public static T New(string @class) {
            return _ctor_mapping[@class]();
        }


        public override Delegate Compile()
        {
            var result = base.Compile();
            _ctor_mapping[_class_name] = (Func<T>)result;
            return result;
        }

        public T Create(string @class)
        {
            return _ctor_mapping[@class]();
        }
        
    }
    public class OopModifier : ScriptStandard<OopModifier>
    {
        internal static ConcurrentDictionary<string, Delegate> _delegate_mapping;
        private Type _oop_type;
        private Dictionary<string, string> _oop_methods_mapping;
        public string Result;
        private MethodOperator _method_handler;
        static OopModifier()
        {
            _delegate_mapping = new ConcurrentDictionary<string, Delegate>();
        }
        public OopModifier(Type oopType) : base()
        {
            _link = this;
            _oop_type = oopType;
            _oop_methods_mapping = new Dictionary<string, string>();
            _method_handler = new MethodOperator(oopType);
            _method_handler.UsingAction = (item) => Using(item);
        }

        public string this[string key]
        {
            get
            {
                return _oop_methods_mapping[key];
            }
            set
            {
                _oop_methods_mapping[key] = _method_handler[key].SetMethod(value);
            }
        }



        /// <summary>
        /// 组装编译
        /// </summary>
        /// <returns></returns>
        public virtual Delegate Compile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _oop_methods_mapping)
            {
                sb.Append(item.Value);
            }

            Result = Using(_oop_type)
                .Namespace("NatashaInterface")
                .Inheritance(_oop_type)
                .MakerHeader()
                .MakerContent(sb.ToString()).Script;

            Type type = ClassBuilder.GetType(Result);
            var tempDelegate = MethodBuilder.NewMethod
                .Using(_oop_type)
                .Using(type)
                .Body($"return new {_class_name}();")
                .Return(_oop_type)
                .Create();

            _delegate_mapping[_class_name] = tempDelegate;
            return tempDelegate;
        }
    }
}
