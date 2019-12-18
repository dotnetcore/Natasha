using Natasha.Operator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Natasha
{
    public class NAssembly
    {

        public Assembly Assembly;
        private readonly HashSet<IScript> _builderCache;
        public readonly AssemblyComplier Options;
        private bool HasChecked;


        public NAssembly(string name) : this()
        {

            Options .AssemblyName = name;

        }


        public NAssembly()
        {

            _builderCache = new HashSet<IScript>();
            Options = new AssemblyComplier();
            HasChecked = false;
            if (Options.AssemblyName == default)
            {
                Options.AssemblyName = Guid.NewGuid().ToString("N");
            }

        }




        public static NAssembly Default
        {

            get { return Create(); }

        }
        /// <summary>
        /// 创建一个程序集操作类
        /// </summary>
        /// <param name="domainName">域名，为空使用系统域</param>
        /// <param name="complieInFile">是否将动态内容编译成DLL</param>
        /// <returns></returns>
        public static NAssembly Create(string domainName = default, bool complieInFile = false)
        {

            NAssembly instance = new NAssembly();
            instance.Options.ComplieInFile = complieInFile;


            if (domainName == default)
            {
                instance.Options.Domain = DomainManagment.Default;
            }
            else
            {
                instance.Options.Domain = DomainManagment.Create(domainName);
            }

            return instance;

        }

        public static NAssembly Create(AssemblyDomain domain, bool complieInFile = false)
        {

            NAssembly instance = new NAssembly();
            instance.Options.ComplieInFile = complieInFile;
            instance.Options.Domain = domain;
            return instance;

        }




        /// <summary>
        /// 在随机域内创建一个操作类
        /// </summary>
        /// <param name="complieInFile">是否将动态内容写入DLL</param>
        /// <returns></returns>
        public static NAssembly Random(bool complieInFile = false)
        {

            NAssembly instance = new NAssembly();
            instance.Options.ComplieInFile = complieInFile;
            instance.Options.Domain = DomainManagment.Random;
            return instance;

        }



        /// <summary>
        /// 移除一个构建类
        /// </summary>
        /// <param name="builder">构建类</param>
        /// <returns></returns>
        public bool Remove(IScript builder)
        {
            return _builderCache.Remove(builder);
        }




        /// <summary>
        /// 直接添加一个合法的类/接口/结构体/枚举
        /// </summary>
        /// <param name="script">脚本代码</param>
        /// <returns></returns>
        public CompilationException AddScript(string script)
        {
            return Options.Add(script);
        }




        /// <summary>
        /// 添加一个带有代码的文件
        /// </summary>
        /// <param name="path">代码文件路径</param>
        /// <returns></returns>
        public CompilationException AddFile(string path)
        {
            return Options.AddFile(path);
        }




        /// <summary>
        /// 创建一个类Operator
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public OopOperator CreateClass(string name = default)
        {
            var @operator = new OopOperator().OopName(name).Namespace(Options.AssemblyName).ChangeToClass();
            _builderCache.Add(@operator);
            return @operator;

        }




        /// <summary>
        /// 创建一个枚举Operator
        /// </summary>
        /// <param name="name">枚举名</param>
        /// <returns></returns>
        public OopOperator CreateEnum(string name = default)
        {

            var @operator = new OopOperator().OopName(name).Namespace(Options.AssemblyName).ChangeToEnum();
            _builderCache.Add(@operator);
            return @operator;

        }




        /// <summary>
        /// 创建一个接口Operator
        /// </summary>
        /// <param name="name">接口名</param>
        /// <returns></returns>
        public OopOperator CreateInterface(string name = default)
        {

            var @operator = new OopOperator().OopName(name).Namespace(Options.AssemblyName).ChangeToInterface();
            _builderCache.Add(@operator);
            return @operator;

        }




        /// <summary>
        /// 创建一个结构体Operator
        /// </summary>
        /// <param name="name">结构体名</param>
        /// <returns></returns>
        public OopOperator CreateStruct(string name = default)
        {

            var @operator = new OopOperator().OopName(name).Namespace(Options.AssemblyName).ChangeToStruct();
            _builderCache.Add(@operator);
            return @operator;

        }




        /// <summary>
        /// 创建一个FastMethodOperator
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public FastMethodOperator CreateFastMethod(string name = default)
        {

            var @operator = new FastMethodOperator().OopName(name);
            @operator.Complier.Domain = Options.Domain;
            return @operator;

        }




        /// <summary>
        /// 创建一个FakeMethodOperator
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public FakeMethodOperator CreateFakeMethod(string name = default)
        {

            var @operator = new FakeMethodOperator().OopName(name);
            @operator.Complier.Domain = Options.Domain;
            return @operator;

        }




        /// <summary>
        /// 进行语法检查
        /// </summary>
        /// <returns></returns>
        public List<CompilationException> Check()
        {

            HasChecked = true;
            foreach (var item in _builderCache)
            {
                Options.Add(item);
            }
            return Options.SyntaxInfos.SyntaxExceptions;

        }




        /// <summary>
        /// 对整个程序集进行编译
        /// </summary>
        /// <returns></returns>
        public Assembly Complier()
        {

            if (!HasChecked)
            {
                Check();
            }


            Assembly = Options.GetAssembly();
            return Assembly;

        }




        /// <summary>
        /// 从编译后的缓存中获取类型
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public Type GetType(string name)
        {

            if (Assembly==default)
            {
                Complier();
            }
            return  Assembly.GetTypes().First(item => item.GetDevelopName() == name);

        }

    }

}
