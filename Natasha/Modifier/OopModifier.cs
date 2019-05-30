using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    /// <summary>
    /// 类构建器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OopModifier<T> : OopModifier
    {
        internal static ConcurrentDictionary<string, Func<T>> _ctor_mapping;
        
        static OopModifier() => _ctor_mapping = new ConcurrentDictionary<string, Func<T>>();

        public OopModifier() : base(typeof(T)){}


        /// <summary>
        /// 编译生成委托
        /// </summary>
        /// <returns></returns>
        public override Delegate Compile()
        {
            return _ctor_mapping[NameScript] = (Func<T>)(base.Compile());
        }




        /// <summary>
        /// 生成实例
        /// </summary>
        /// <param name="class">类名</param>
        /// <returns></returns>
        public T Create(string @class)
        {
            if (!_ctor_mapping.ContainsKey(@class)){Compile();}
            return _ctor_mapping[@class]();
        }
    }




    /// <summary>
    /// 类构建器
    /// </summary>
    public class OopModifier : ClassContentTemplate<OopModifier>
    {

        internal readonly static ConcurrentDictionary<string, Delegate> _delegate_mapping;
        private readonly Type _oop_type;
        private readonly Dictionary<string, string> _oop_methods_mapping;

        public string Result;
        public Type TargetType;

        static OopModifier()=> _delegate_mapping = new ConcurrentDictionary<string, Delegate>();

        public OopModifier(Type oopType) : base()
        {
            Link = this;
            _oop_type = oopType;
            _oop_methods_mapping = new Dictionary<string, string>();
        }




        private bool _is_override;
        /// <summary>
        /// 重写函数
        /// </summary>
        /// <returns></returns>
        public OopModifier Override()
        {
            _is_override = true;
            return this;
        }




        private bool _is_new;
        /// <summary>
        /// 覆盖函数
        /// </summary>
        /// <returns></returns>
        public OopModifier New()
        {
            _is_new = true;
            return this;
        }




        /// <summary>
        /// 操作当前函数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                return _oop_methods_mapping[key];
            }
            set
            {

                //获取反射信息
                var reflectMethodInfo = _oop_type.GetMethod(key);
                if (reflectMethodInfo == null)
                {
                    throw new Exception($"无法在{_oop_type.Name}中找到{key}函数！");
                }


                //填装引用
                Using(reflectMethodInfo);


                //使用伪造函数模板
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


                _oop_methods_mapping[key] = template.UseMethod(reflectMethodInfo).MethodContent(value).MethodScript;
            }
        }



        /// <summary>
        /// 组装编译
        /// </summary>
        /// <returns></returns>
        public virtual Delegate Compile()
        {

            StringBuilder sb = new StringBuilder();


            //填充类的内容
            foreach (var item in _oop_methods_mapping)
            {
                sb.Append(item.Value);
            }


            //生成整类脚本
            Result = Using(_oop_type)
                .Namespace("NatashaInterface")
                .ClassAccess(AccessTypes.Public)
                .Inheritance(_oop_type)
                .ClassBody(sb.ToString())
                .Builder();


            //获取类型
            TargetType = ClassBuilder.GetType(Result);


            //返回委托
            return _delegate_mapping[NameScript] = CtorOperator.NewDelegate(TargetType);
        }




        /// <summary>
        /// 根据类名生成委托
        /// </summary>
        /// <typeparam name="T">强委托类型</typeparam>
        /// <param name="class">类名</param>
        /// <returns></returns>
        public T Create<T>(string @class)
        {
            if (!_delegate_mapping.ContainsKey(@class)){Compile();}
            return ((Func<T>)_delegate_mapping[@class])();
        }
    }
}
