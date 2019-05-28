using Natasha.Engine.Builder;
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

        public override Delegate Compile()
        {
            var result = base.Compile();
            _ctor_mapping[NameScript] = (Func<T>)result;
            return result;
        }

        public T Create(string @class)
        {
            if (!_ctor_mapping.ContainsKey(@class))
            {
                Compile();
            }
            return _ctor_mapping[@class]();
        }

    }
    public class OopModifier : ClassContentTemplate<OopModifier>
    {
        internal readonly static ConcurrentDictionary<string, Delegate> _delegate_mapping;
        private readonly Type _oop_type;
        private readonly Dictionary<string, string> _oop_methods_mapping;
        public string Result;
        public Type TargetType;
        static OopModifier()
        {
            _delegate_mapping = new ConcurrentDictionary<string, Delegate>();
        }
        public OopModifier(Type oopType) : base()
        {
            Link = this;
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
                Using(info);

                var template = FakeMethod.New;
                if (_is_override)
                {
                    template.MethodModifier(Modifiers.Override);
                    _is_override = false;
                }
                if (_is_new)
                {
                    template.MethodModifier(Modifiers.New);
                    _is_new = false;
                }
                _oop_methods_mapping[key] = template.UseMethod(info).MethodContent(value).MethodScript;
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
                .ClassAccess(AccessTypes.Public)
                .Inheritance(_oop_type)
                .ClassBody(sb.ToString())
                .Builder();

            TargetType = ClassBuilder.GetType(Result);
            var tempDelegate = CtorOperator.NewDelegate(TargetType);
            _delegate_mapping[NameScript] = tempDelegate;
            return tempDelegate;
        }

        public T Create<T>(string @class)
        {
            if (!_delegate_mapping.ContainsKey(@class))
            {
                Compile();
            }
            return ((Func<T>)_delegate_mapping[@class])();
        }
    }
}
