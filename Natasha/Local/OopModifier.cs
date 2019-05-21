using Natasha.Engine.Builder;
using Natasha.Engine.Template;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public static T New(string @class)
        {
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
    public class OopModifier : BuilderStandard<OopModifier>
    {
        internal static ConcurrentDictionary<string, Delegate> _delegate_mapping;
        private Type _oop_type;
        private Dictionary<string, string> _oop_methods_mapping;
        public string Result;
        static OopModifier()
        {
            _delegate_mapping = new ConcurrentDictionary<string, Delegate>();
        }
        public OopModifier(Type oopType) : base()
        {
            _link = this;
            _oop_type = oopType;
            _oop_methods_mapping = new Dictionary<string, string>();
        }
        private bool _is_override;
        public OopModifier Override()
        {
            _is_override = true;
            return this;
        }
        private bool _is_new;
        public OopModifier New()
        {
            _is_new = true;
            return this;
        }


        public string this[string key]
        {
            get
            {
                return _oop_methods_mapping[key];
            }
            set
            {
                var info = _oop_type.GetMethod(key);
                if (info == null)
                {
                    throw new Exception($"无法在{_oop_type.Name}中找到{key}函数！");
                }
                Method(info);
                MethodTemplate template = new MethodTemplate(info);
                if (_is_override)
                {
                    template.Override();
                    _is_override = false;
                }
                if (_is_new)
                {
                    template.New();
                    _is_new = false;
                }
                _oop_methods_mapping[key] = template.Create(value);
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
