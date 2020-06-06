using Natasha.Error;
using Natasha.CSharp.Template;
using System;

namespace Natasha.CSharp.Builder
{

    public class MethodBuilder : MethodBuilder<MethodBuilder>
    {

        public MethodBuilder()
        {
            Link = this;
        }

    }


    public class MethodBuilder<T> : DelegateTemplate<T> where T : MethodBuilder<T>, new()
    {


        private readonly OopBuilder _oopHandler;
        public CompilationException Exception;

        public MethodBuilder()
        {

            _oopHandler = new OopBuilder();
            Body(default);
            Init();

        }




        public T Using<S>()
        {

            _oopHandler.Using<S>();
            return Link;

        }
        public T Using(Type @using)
        {

            _oopHandler.Using(@using);
            return Link;

        }
        public T Using(string @using)
        {

            _oopHandler.Using(@using);
            return Link;

        }
        public T Using(NamespaceConverter @using)
        {

            _oopHandler.Using(@using);
            return Link;

        }
        public T Using(NamespaceConverter[] @using)
        {

            _oopHandler.Using(@using);
            return Link;

        }




        public virtual void Init()
        {

        }




        public T ClassOptions(Action<OopBuilder> acion)
        {

            acion?.Invoke(_oopHandler);
            return Link;

        }




        /// <summary>
        /// 设置类名
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public T ClassName(string className)
        {

            _oopHandler.Name(className);
            return Link;

        }




        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespace">命名空间字符串</param>
        /// <returns></returns>
        public T Namesapce(string @namespace)
        {

            _oopHandler.Namespace(@namespace);
            return Link;

        }




        public Delegate Compile(object binder = null)
        {

            if (_oopHandler.NamespaceScript == default)
            {
                _oopHandler.HiddenNamespace();
            }


            _oopHandler.BodyAppend(Script);
            Exception = AssemblyBuilder.Syntax.Add(_oopHandler);
            if (!Exception.HasError)
            {
                var @delegate = AssemblyBuilder.GetDelegateFromShortName(_oopHandler.NameScript, NameScript, DelegateType, binder);
                if (@delegate == null)
                {
                    Exception = AssemblyBuilder.Exceptions[0];
                }
                return @delegate;
            }
            return null;

        }





        public S Compile<S>(object binder = null) where S : Delegate
        {


            if (_oopHandler.NamespaceScript == default)
            {

                _oopHandler.HiddenNamespace();

            }


            //自动判别是否有手动指定方法参数，若没有则使用方法的参数
            var method = typeof(S).GetMethod("Invoke");
            if (ParametersScript.Length == 0)
            {

                Param(method);

            }


            //自动判别返回值类型，若没有则使用委托的返回类型
            if (_type != method.ReturnType)
            {

                this.Return(method.ReturnType);

            }


            _oopHandler.BodyAppend(Script);
            Exception = AssemblyBuilder.Syntax.Add(_oopHandler);
            if (!Exception.HasError)
            {
                S @delegate = AssemblyBuilder.GetDelegateFromShortName<S>(_oopHandler.NameScript, NameScript, binder);
                if (@delegate == null)
                {
                    Exception = AssemblyBuilder.Exceptions[0];
                }
                return @delegate;
            }
            return null;

        }

    }

}
