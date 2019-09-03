using Natasha.Complier.Model;
using Natasha.Log;
using System;
using System.Linq;
using System.Reflection;

namespace Natasha.Complier
{
    public abstract partial class IComplier
    {

        public readonly CompilationException Exception;
        public readonly ComplierOption ComplierInfos;


        public IComplier()
        {
            Exception = new CompilationException();
            ComplierInfos = new ComplierOption();
        }




        public string Name
        {
            get
            {
                return ComplierInfos.AssemblyName;
            }
            set
            {
                ComplierInfos.AssemblyName = value;
            }
        }




        /// <summary>
        /// 获取编译后的程序集
        /// </summary>
        /// <returns></returns>
        public Assembly GetAssembly()
        {

            if (ComplierInfos.Trees.Count == 0)
            {
                return null;
            }


            if (Name == default)
            {
                Name = Guid.NewGuid().ToString("N");
            }

            
            var result = StreamComplier(ComplierInfos);
            Assembly assembly = result.Assembly;
            if (result.Compilation != null)
            {
                if (assembly == default || assembly == null)
                {

                    Exception.Diagnostics.AddRange(result.Errors);
                    Exception.ErrorFlag = ComplieError.Complie;
                    Exception.Message = "发生错误,无法生成程序集！";


                    NError logError = new NError();
                    logError.Handler(result.Compilation, Exception.Diagnostics);
                    Exception.Log = logError.Buffer.ToString();
                    if (NError.Enabled) { logError.Write(); }

                }
                else
                {

                    NSucceed logSucceed = new NSucceed();
                    logSucceed.Handler(result.Compilation);
                    Exception.ErrorFlag = ComplieError.None;
                    Exception.Log = logSucceed.Buffer.ToString();
                    if (NSucceed.Enabled) { logSucceed.Write(); }

                }
            }
            return assembly;

        }




        /// <summary>
        /// 获取编译后的类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns></returns>
        public Type GetType(string typeName)
        {

            Assembly assembly = GetAssembly();
            if (assembly == null)
            {
                return null;
            }


            var type = assembly.GetTypes().First(item => item.Name == typeName);
            if (type == null)
            {

                Exception.ErrorFlag = ComplieError.Type;
                Exception.Message = $"发生错误,无法从程序集{assembly.FullName}中获取类型{typeName}！";

            }


            return type;

        }




        /// <summary>
        /// 获取编译后的方法元数据
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public MethodInfo GetMethod(string typeName, string methodName = null)
        {

            var type = GetType(typeName);
            if (type == null)
            {
                return null;
            }


            var info = type.GetMethod(methodName);
            if (info == null)
            {

                Exception.ErrorFlag = ComplieError.Method;
                Exception.Message = $"发生错误,无法从类型{typeName}中找到{methodName}方法！";


            }


            return info;

        }




        /// <summary>
        /// 获取编译后的委托
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public Delegate GetDelegate(string typeName, string methodName, Type delegateType, object binder = null)
        {

            var info = GetMethod(typeName, methodName);
            if (info == null)
            {
                return null;
            }



            try
            {

                return info.CreateDelegate(delegateType, binder);

            }
            catch (Exception ex)
            {

                Exception.ErrorFlag = ComplieError.Delegate;
                Exception.Message = $"发生错误,无法从方法{methodName}创建{delegateType.GetDevelopName()}委托！";

            }


            return null;

        }

        public T GetDelegate<T>(string typeName, string methodName, object binder = null) where T : Delegate
        {

            return (T)GetDelegate(typeName, methodName, typeof(T));

        }


    }

}
