using Natasha.Builder;
using Natasha.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Natasha
{
    public class NAssembly : CompilerTemplate<NAssembly>
    {

        public Assembly Assembly;
        private readonly HashSet<IScript> _builderCache;
        private bool HasChecked;


        public NAssembly(string name) : this()
        {

            Compiler.AssemblyName = name;

        }




        public NAssembly()
        {

            _builderCache = new HashSet<IScript>();
            HasChecked = false;
            if (Compiler.AssemblyName == default)
            {
                Compiler.AssemblyName = Guid.NewGuid().ToString("N");
            }

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
            return Compiler.Add(script);
        }




        /// <summary>
        /// 添加一个带有代码的文件
        /// </summary>
        /// <param name="path">代码文件路径</param>
        /// <returns></returns>
        public CompilationException AddFile(string path)
        {
            return Compiler.AddFile(path);
        }



        private T GetBaseOopHandler<T>(string name = default) where T : OopBuilder<T>, new()
        {
            var @operator = new T().DefinedName(name).Namespace(Compiler.AssemblyName);
            _builderCache.Add(@operator);
            return @operator;
        }
        private S GetBaseDelegateHandler<S>(string name = default) where S : MethodBuilder<S>, new()
        {
            var @operator = new S().DefinedName(name).ClassOptions(item=>item.Namespace(Compiler.AssemblyName));
            @operator.Compiler.Domain = Compiler.Domain;
            _builderCache.Add(@operator);
            return @operator;
        }




        /// <summary>
        /// 创建一个类Operator
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public NClass CreateClass(string name = default)
        {

            return GetBaseOopHandler<NClass>(name);

        }




        /// <summary>
        /// 创建一个枚举Operator
        /// </summary>
        /// <param name="name">枚举名</param>
        /// <returns></returns>
        public NEnum CreateEnum(string name = default)
        {

            return GetBaseOopHandler<NEnum>(name);

        }




        /// <summary>
        /// 创建一个接口Operator
        /// </summary>
        /// <param name="name">接口名</param>
        /// <returns></returns>
        public NInterface CreateInterface(string name = default)
        {

            return GetBaseOopHandler<NInterface>(name);

        }




        /// <summary>
        /// 创建一个结构体Operator
        /// </summary>
        /// <param name="name">结构体名</param>
        /// <returns></returns>
        public NStruct CreateStruct(string name = default)
        {

            return GetBaseOopHandler<NStruct>(name);

        }




        /// <summary>
        /// 创建一个FastMethodOperator
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public FastMethodOperator CreateFastMethod(string name = default)
        {

            return GetBaseDelegateHandler<FastMethodOperator>(name);

        }




        /// <summary>
        /// 创建一个FakeMethodOperator
        /// </summary>
        /// <param name="name">类名</param>
        /// <returns></returns>
        public FakeMethodOperator CreateFakeMethod(string name = default)
        {

            return GetBaseDelegateHandler<FakeMethodOperator>(name);

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
                Compiler.Add(item);
            }
            return Compiler.SyntaxInfos.SyntaxExceptions;

        }




        /// <summary>
        /// 对整个程序集进行编译
        /// </summary>
        /// <returns></returns>
        public Assembly Compile()
        {

            if (!HasChecked)
            {
                Check();
            }


            Assembly = Compiler.GetAssembly();
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
                Compile();
            }
            return  Assembly.GetTypes().First(item => item.GetDevelopName() == name);

        }

    }

}
