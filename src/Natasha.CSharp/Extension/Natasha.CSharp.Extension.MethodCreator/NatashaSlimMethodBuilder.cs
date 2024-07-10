using System.Text;

namespace Natasha.CSharp.Extension.MethodCreator
{
    public class NatashaSlimMethodBuilder
    {
        public readonly AssemblyCSharpBuilder Builder;
        public readonly string Script;
        public readonly HashSet<string> Usings;
        private HashSet<string>? _exceptUsings;
        private object[]? _privateObjects;
        private Action<NatashaLoadContext>? _ctxConfig;
        private Action<AssemblyCSharpBuilder>? _builderConfig;
        public NatashaSlimMethodBuilder(string script)
        {
            Usings = [];
            Builder = new AssemblyCSharpBuilder();
            Script = script;
        }
        public NatashaSlimMethodBuilder WithPrivateAccess(params object[] objs)
        {
            _privateObjects = objs;
            return this;
        }

        public NatashaSlimMethodBuilder ConfigBuilder(Action<AssemblyCSharpBuilder> config)
        {
            _builderConfig = config;
            return this;
        }
        public NatashaSlimMethodBuilder ConfigBuilder(Action<NatashaLoadContext> config)
        {
            _ctxConfig = config;
            return this;
        }
        public NatashaSlimMethodBuilder WithSimpleBuilder()
        {
            Builder.UseRandomLoadContext();
            Builder.UseSimpleMode();
            Builder.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>());
            return this;
        }
        public NatashaSlimMethodBuilder WithSmartBuilder()
        {
            Builder.UseRandomLoadContext();
            Builder.UseSmartMode();
            return this;
        }

        public NatashaSlimMethodBuilder WithUsings(params string[] usings)
        {
            Usings.UnionWith(usings);
            return this;
        }
        public NatashaSlimMethodBuilder WithoutUsings(params string[] usings)
        {
            Builder.AppendExceptUsings(usings);
            return this;
        }
        public NatashaSlimMethodBuilder WithMetadata<T>()
        {
            Builder.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<T>());
            return this;
        }

        public NatashaSlimMethodBuilder WithMetadata(params Type[] types)
        {
            if (types!=null && types.Length > 0)
            {
                foreach (var type in types)
                {
                    Builder.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode(type));
                }
            }
            return this;
        }

        public T ToDelegate<T>(string modifier = "") where T : Delegate
        {
            _builderConfig?.Invoke(Builder);
            _ctxConfig?.Invoke(Builder.LoadContext);     
            
            var className = $"N{Guid.NewGuid():N}";
            var methodInfo = typeof(T).GetMethod("Invoke")!;

            var returnTypeScript = methodInfo.ReturnType.GetDevelopName();
            var parameterScript = new StringBuilder();

            var methodParameters = methodInfo.GetParameters();
            for (int i = 0; i < methodParameters.Length; i += 1)
            {
                var paramType = methodParameters[i].ParameterType;
                Builder.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode(paramType));
                parameterScript.Append($"{paramType.GetDevelopName()} arg{i + 1},");
            }
            if (parameterScript.Length > 0)
            {
                parameterScript.Length -= 1;
            }

            StringBuilder usingCode = new();
            foreach (var item in Usings)
            {

                usingCode.AppendLine($"using {item};");

            }
            var fullScript = $"{usingCode} public static class {className} {{ public static {(modifier ?? string.Empty)} {returnTypeScript} Invoke({parameterScript}){{ {Script} }} }}";
            if (_privateObjects != null)
            {
                Builder.WithPrivateAccess();
                Builder.Add(fullScript.ToAccessPrivateTree(_privateObjects));
            }
            else
            {
                Builder.Add(fullScript);
            }
            var asm = Builder.GetAssembly();
            var type = asm.GetType(className);
            if (type != null)
            {
                return (T)Delegate.CreateDelegate(typeof(T), type.GetMethod("Invoke")!);
            }
            throw new Exception($"未找到 {className} 类型！");
        }
        public T ToAsyncDelegate<T>() where T : Delegate
        {
            return ToDelegate<T>("async");
        }
        public T ToUnsafeDelegate<T>() where T : Delegate
        {
            return ToDelegate<T>("unsafe");
        }
        public T ToUnsafeAsyncDelegate<T>() where T : Delegate
        {
            return ToDelegate<T>("unsafe async");
        }
    }
}
