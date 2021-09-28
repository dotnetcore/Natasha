using Natasha.Error;
using Natasha.CSharp.Template;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics;

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


        public readonly OopBuilder OopHandler;
        public NatashaException Exception;

        public MethodBuilder()
        {

            OopHandler = new OopBuilder();
            Body(default);
            Init();

        }


#if NET5_0
        public T SkipInit()
        {
            this.AttributeAppend<SkipLocalsInitAttribute>();
            return Link;
        }
#endif


        public T Using<S>()
        {

            OopHandler.Using<S>();
            return Link;

        }
        public T Using(Type @using)
        {

            OopHandler.Using(@using);
            return Link;

        }
        public T Using(string @using)
        {

            OopHandler.Using(@using);
            return Link;

        }

        public T Using(HashSet<string> @using)
        {

            OopHandler.Using(@using);
            return Link;

        }
        public T Using(NamespaceConverter @using)
        {

            OopHandler.Using(@using);
            return Link;

        }
        public T Using(NamespaceConverter[] @using)
        {

            OopHandler.Using(@using);
            return Link;

        }




        public virtual void Init()
        {

        }




        public T ClassOptions(Action<OopBuilder> acion)
        {

            acion?.Invoke(OopHandler);
            return Link;

        }




        /// <summary>
        /// 设置类名
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public T ClassName(string className)
        {

            OopHandler.Name(className);
            return Link;

        }




        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespace">命名空间字符串</param>
        /// <returns></returns>
        public T Namesapce(string @namespace)
        {

            OopHandler.Namespace(@namespace);
            return Link;

        }




        public Delegate Compile(object target = null)
        {

            Using(AssemblyBuilder.Compiler.Domain.GetReferenceElements().ToArray());
            if (OopHandler.NamespaceScript == default)
            {
                OopHandler.HiddenNamespace();
            }


            OopHandler.BodyAppend(Script);
            Exception = AssemblyBuilder.Add(OopHandler);


            if (!Exception.HasError)
            {
                var @delegate = AssemblyBuilder.GetDelegateFromShortName(OopHandler.NameScript, NameScript, DelegateType, target);
                if (@delegate == null)
                {
                    Exception = AssemblyBuilder.Exceptions[0];
                }
                return @delegate;
            }
            return null;

        }





        public S Compile<S>(object target = null) where S : Delegate
        {
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            Using(AssemblyBuilder.Compiler.Domain.GetReferenceElements());
            if (OopHandler.NamespaceScript == default)
            {

                OopHandler.HiddenNamespace();

            }
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[ Using ]", "Using填充耗时", 1);
            stopwatch.Restart();
#endif
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
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[Delegate]", "委托信息反解耗时", 1);
            Console.WriteLine();
            stopwatch.Restart();
#endif
            //Mark : 11M Memory
            OopHandler.BodyAppend(Script);
            Exception = AssemblyBuilder.Add(OopHandler);
            if (!Exception.HasError)
            {
                S @delegate = AssemblyBuilder.GetDelegateFromShortName<S>(OopHandler.NameScript, NameScript, target);
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
