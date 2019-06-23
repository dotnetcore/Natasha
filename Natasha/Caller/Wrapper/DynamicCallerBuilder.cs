using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha.Caller.Wrapper
{
    public class DynamicCallerBuilder<T>
    {
        public static readonly Func<DynamicBase> Ctor;
        static DynamicCallerBuilder() => Ctor = DynamicCallerBuilder.InitType(typeof(T));
    }


    public class DynamicCallerBuilder
    {


        public static readonly ConcurrentDictionary<Type, Func<DynamicBase>> TypeCreatorMapping;
        static DynamicCallerBuilder()
        {
            TypeCreatorMapping = new ConcurrentDictionary<Type, Func<DynamicBase>>();
        }

        public static DynamicBase Ctor(Type type)
        {
            if (!TypeCreatorMapping.ContainsKey(type))
            {
                InitType(type);
            }
            return TypeCreatorMapping[type]();
        }


        public static Func<DynamicBase> InitType(Type type)
        {
            string innerBody = "InnerDynamic" + type.GetAvailableName();
            string entityClassName = type.GetDevelopName();
            string className = "NatashaDynamic" + type.GetAvailableName();


            ClassBuilder builder = new ClassBuilder();
            StringBuilder body = new StringBuilder();


            body.Append($@" 
                    public readonly static ConcurrentDictionary<string,Func<{entityClassName},DynamicBase>> LinkMapping;
                    static {className}(){{

                        LinkMapping = new ConcurrentDictionary<string,Func<{entityClassName},DynamicBase>>();
                       

                        var fields = typeof({entityClassName}).GetFields();
                        for (int i = 0; i < fields.Length; i+=1)
                        {{
                            if(!fields[i].FieldType.IsOnceType())
                            {{
                                LinkMapping[fields[i].Name] = FastMethodOperator
                                                                .New
                                                                .Using(fields[i].FieldType)
                                                                .Using(""Natasha.Caller"")
                                                                .Param<{entityClassName}>(""obj"")
                                                                .MethodBody($@""return obj.{{fields[i].Name}}.Caller<{{fields[i].FieldType.GetDevelopName()}}>();"")
                                                                .Return<DynamicBase>()
                                                                .Complie<Func<{entityClassName}, DynamicBase>>();
                            }}
                        }}


                        var props = typeof({entityClassName}).GetProperties(); 
                        for (int i = 0; i < props.Length; i+=1)
                        {{
                            if(!props[i].PropertyType.IsOnceType())
                            {{
                                LinkMapping[props[i].Name] = FastMethodOperator
                                                                .New
                                                                .Using(props[i].PropertyType)
                                                                .Using(""Natasha.Caller"")
                                                                .Param<{entityClassName}>(""obj"")
                                                                .MethodBody($@""return obj.{{props[i].Name}}.Caller<{{props[i].PropertyType.GetDevelopName()}}>();"")
                                                                .Return<DynamicBase>()
                                                                .Complie<Func<{entityClassName}, DynamicBase>>();
                            }}
                        }}
                    }}");

            body.Append($@" 
                    public override void New(){{
                         Instance = new {type.GetDevelopName()}();
                    }}");

            body.Append($@" 
                    public override DynamicBase Get(string name){{
                         return LinkMapping[name](Instance);
                    }}");

            body.Append($@" 
                    public override T Get<T>(string name){{
                        return {innerBody}<T>.GetterMapping[name](Instance);
                    }}");

            body.Append($@" 
                    public override T Get<T>(){{
                        return {innerBody}<T>.GetterMapping[_current_name](Instance);
                    }}");

            body.Append($@" 
                    public override void Set<T>(string name,T value){{
                        {innerBody}<T>.SetterMapping[name](Instance,value);
                    }}");
            body.Append($@" 
                    public override void Set<T>(T value){{
                        {innerBody}<T>.SetterMapping[_current_name](Instance,value);
                    }}");

            Type tempClass = builder
                    .Using(type)
                    .Using("System")
                    .Using("System.Collections.Concurrent")
                    .ClassAccess(AccessTypes.Public)
                    .ClassName(className)
                    .Namespace("NatashaDynamic")
                    .Inheritance(GenericBuilder.GetType(typeof(DynamicBase<>),type))
                    .ClassBody(body + InnerTemplate.GetNormalInnerString(innerBody, entityClassName))
                    .GetType();

            return TypeCreatorMapping[type] = (Func<DynamicBase>)CtorBuilder.NewDelegate(tempClass);
        }
    }
}
