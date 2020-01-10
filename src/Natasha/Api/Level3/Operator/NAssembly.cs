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




        #region 指定字符串域创建以及参数
        public static NAssembly Create(string domainName, ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(domainName, error, target);

        }

        public static NAssembly Create(string domainName, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            if (domainName == default || domainName.ToLower() == "default")
            {
                return Create(DomainManagment.Default, target, error);
            }
            else
            {
                return Create(DomainManagment.Create(domainName), target, error);
            }

        }
        #endregion
        #region 指定域创建以及参数
        public static NAssembly Create(AssemblyDomain domain, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(domain, target, error);

        }

        public static NAssembly Create(AssemblyDomain domain, ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            NAssembly instance = new NAssembly();
            instance.Options.EnumCRError = error;
            instance.Options.EnumCRTarget = target;
            instance.Options.Domain = domain;
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static NAssembly Default()
        {

            return Create(DomainManagment.Default, ComplierResultTarget.Stream);

        }

        public static NAssembly Default(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Default, target, error);

        }

        public static NAssembly Default(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Default, target, error);

        }
        #endregion
        #region 随机域创建以及参数
        public static NAssembly Random()
        {

            return Create(DomainManagment.Random, ComplierResultTarget.Stream);

        }


        public static NAssembly Random(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Random, target, error);

        }


        public static NAssembly Random(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Random, target, error);

        }
        #endregion




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
