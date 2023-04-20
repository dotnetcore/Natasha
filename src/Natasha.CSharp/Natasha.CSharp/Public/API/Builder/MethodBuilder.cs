using Natasha.CSharp.Template;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

        public MethodBuilder()
        {
            OopHandler = new OopBuilder();
            Init();

        }



        public T SkipInit()
        {

            this.OopHandler.AssemblyBuilder.ConfigCompilerOption(item => item.RemoveIgnoreAccessibility());
            this.AttributeAppend<SkipLocalsInitAttribute>();
            return Link;
        }



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
        public T Using(NamespaceConverter[]? @using)
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




        public Delegate Compile(object? target = null)
        {
#if DEBUG
            Stopwatch stopwatch = new();
            stopwatch.Start();
#endif
            OopHandler.AssemblyBuilder = this.AssemblyBuilder;
            if (OopHandler.NamespaceScript == default)
            {
                OopHandler.HiddenNamespace();
            }

#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[ Using ]", "Using填充耗时", 1);
#endif
            OopHandler.BodyAppend(GetScript());
            AssemblyBuilder.Add(OopHandler.GetScript());
            var assembly = AssemblyBuilder.GetAssembly();
            return assembly.GetDelegateFromShortName(OopHandler.NameScript, NameScript, DelegateType, target);

        }





        public S Compile<S>(object? target = null) where S : Delegate
        {
#if DEBUG
            Stopwatch stopwatch = new();
            stopwatch.Start();
#endif

            OopHandler.AssemblyBuilder = this.AssemblyBuilder;
            if (OopHandler.NamespaceScript == default)
            {
                OopHandler.HiddenNamespace();
            }
#if DEBUG
            stopwatch.RestartAndShowCategoreInfo("[ Using ]", "Using填充耗时", 1);
#endif
            //自动判别是否有手动指定方法参数，若没有则使用方法的参数
            var method = typeof(S).GetMethods()[0]!;
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
#endif
            //Mark : 11M Memory
            OopHandler.BodyAppend(GetScript());
            AssemblyBuilder.Add(OopHandler.GetScript());
            var assembly = AssemblyBuilder.GetAssembly();
            return assembly.GetDelegateFromShortName<S>(OopHandler.NameScript, NameScript, target);

        }

    }

}
