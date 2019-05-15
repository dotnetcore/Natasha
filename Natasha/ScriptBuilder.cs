using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class ScriptBuilder
    {
        private List<KeyValuePair<Type, string>> _parameters;
        private List<Type> _parameters_types;
        private Type _return_type;
        private string _text;
        private Type _delegate_type;
        public static Action<string> SingleError;
        public ScriptBuilder()
        {
            _class_name = "N"+Guid.NewGuid().ToString("N");
            _namespace = new StringBuilder();
            _parameters = new List<KeyValuePair<Type, string>>();
            _parameters_types = new List<Type>();
            _return_type = null;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="key">参数名字</param>
        /// <returns></returns>
        public ScriptBuilder Param<T>(string key)
        {
            return Param(typeof(T), key);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="key">参数名字</param>
        /// <returns></returns>
        public ScriptBuilder Param(Type type,string key)
        {
            _parameters_types.Add(type);
            _parameters.Add(new KeyValuePair<Type, string>(type, key));
            return this;
        }

        /// <summary>
        /// 写函数名
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public ScriptBuilder Body(string text)
        {
            _text = text;
            return this;
        }




        /// <summary>
        /// 设置返回类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns></returns>
        public ScriptBuilder Return<T>()
        {
            return Return(typeof(T));
        }
        /// <summary>
        /// 设置返回类型,并生成运行时委托
        /// </summary>
        /// <param name="type">返回类型</param>
        /// <returns></returns>
        public ScriptBuilder Return(Type type=null)
        {
            _return_type = type;
            //根据参数，生成动态委托类型
            _delegate_type = DelegateBuilder.GetDelegate(_parameters_types.ToArray(), type);
            return this;
        }

        /// <summary>
        /// 返回动态委托
        /// </summary>
        /// <returns></returns>
        public Delegate Create()
        {
            //生成完整动态代码
            string body = GetScriptString();
            //返回运行时委托
            return GetRuntimeMethodDelegate(_class_name, body, _delegate_type);
        }
        public T Create<T>() where T : Delegate
        {
            //生成完整动态代码
            string body = GetScriptString();
            //返回运行时委托
            return (T)GetRuntimeMethodDelegate(_class_name, body, typeof(T));
        }


        public static Delegate GetRuntimeMethodDelegate(string className,string body,Type delegateType)
        {
            //写入分析树
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(body);

            //添加程序及引用
            var _ref = DependencyContext.Default.CompileLibraries
                                    .SelectMany(cl => cl.ResolveReferencePaths())
                                    .Select(asm => MetadataReference.CreateFromFile(asm));


            //创建dll
            string fileName = $"{className}.dll";

            //创建语言编译
            CSharpCompilation compilation = CSharpCompilation.Create(
                fileName,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                references: _ref);


            Assembly compiledAssembly;
            using (MemoryStream stream = new MemoryStream())
            {
                EmitResult compileResult = compilation.Emit(stream);

                if (compileResult.Success)
                {
                    //从内存中加载程序集
                    compiledAssembly = Assembly.Load(stream.GetBuffer());
                    Type targetType = compiledAssembly.GetType(className);
                    return targetType.GetMethod("DynimacMethod").CreateDelegate(delegateType);
                }
                else
                {
                    foreach (var item in compileResult.Diagnostics)
                    {
                        SingleError?.Invoke(item.GetMessage());
                    }
                    return null;
                    //throw new Exception("你的.csproj文件里，需要有：<PreserveCompilationContext>true</PreserveCompilationContext>")
                }
            }
        }


        private StringBuilder _namespace;
        private string _class_name;
        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <returns></returns>
        public ScriptBuilder Namespaces(params string[] namespaces)
        {
            for (int i = 0; i < namespaces.Length; i++)
            {
                _namespace.Append($"using {namespaces[i]};");
            }
            return this;
        }
        public ScriptBuilder Namespace<T>()
        {
            _namespace.Append($"using {typeof(T).Namespace};");
            return this;
        }
        public ScriptBuilder Namespace(Type type)
        {
            _namespace.Append($"using {type.Namespace};");
            return this;
        }

        private string GetScriptString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_namespace);
            sb.Append($"public class {_class_name}");
            sb.Append("{");
            sb.Append($"public static ");
            if (_return_type==null)
            {
                sb.Append("void");
            }
            else
            {
                sb.Append(_return_type.Name);
            }
            sb.Append(" DynimacMethod(");
            if (_parameters.Count>0)
            {
                sb.Append($"{_parameters[0].Key.Name} {_parameters[0].Value}");
                for (int i = 1; i < _parameters.Count; i++)
                {
                    sb.Append($",{_parameters[i].Key.Name} {_parameters[i].Value}");
                }
            }
            sb.Append("){");
            sb.Append(_text);
            sb.Append("}}");
            return sb.ToString();
        }

    }
}
