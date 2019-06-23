using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha.Caller.Wrapper
{

    public class DynamicStaticCallerBuilder
    {
        public static readonly ConcurrentDictionary<Type, Func<DynamicBase>> TypeCreatorMapping;
        static DynamicStaticCallerBuilder()
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
            string innerClassName = "InnerDynamicStatic" + type.GetAvailableName();
            string entityClassName = type.GetDevelopName();
            string className = "NatashaDynamicStatic" + type.GetAvailableName();


            ClassBuilder builder = new ClassBuilder();
            StringBuilder body = new StringBuilder();


            body.Append($@" 
                    public readonly static ConcurrentDictionary<string,Func<DynamicBase>> LinkMapping;
                    static {className}(){{

                        LinkMapping = new ConcurrentDictionary<string,Func<DynamicBase>>();
                       

                        var fields = typeof({entityClassName}).GetFields();
                        for (int i = 0; i < fields.Length; i+=1)
                        {{
                            if(!fields[i].FieldType.IsOnceType())
                            {{
                                LinkMapping[fields[i].Name] = FastMethodOperator
                                                                .New
                                                                .Using(fields[i].FieldType)
                                                                .Using(""Natasha.Caller"")
                                                                .MethodBody($@""return {entityClassName}.{{fields[i].Name}}.Caller<{{fields[i].FieldType.GetDevelopName()}}>();"")
                                                                .Return<DynamicBase>()
                                                                .Complie<Func<DynamicBase>>();
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
                                                                .MethodBody($@""return  {entityClassName}.{{props[i].Name}}.Caller<{{props[i].PropertyType.GetDevelopName()}}>();"")
                                                                .Return<DynamicBase>()
                                                                .Complie<Func<DynamicBase>>();
                            }}
                        }}
                    }}");

            body.Append($@" 
                    public override DynamicBase Get(string name){{
                         return LinkMapping[name]();
                    }}");

            body.Append($@" 
                    public override T Get<T>(string name){{
                        return {innerClassName}<T>.GetterMapping[name]();
                    }}");

            body.Append($@" 
                    public override T Get<T>(){{
                        return {innerClassName}<T>.GetterMapping[_current_name]();
                    }}");

            body.Append($@" 
                    public override void Set<T>(string name,T value){{
                        {innerClassName}<T>.SetterMapping[name](value);
                    }}");
            body.Append($@" 
                    public override void Set<T>(T value){{
                        {innerClassName}<T>.SetterMapping[_current_name](value);
                    }}");

            Type tempClass = builder
                    .Using(type)
                    .Using("System")
                    .Using("System.Collections.Concurrent")
                    .ClassAccess(AccessTypes.Public)
                    .ClassName(className)
                    .Namespace("NatashaDynamicStatic")
                    .Inheritance<DynamicBase>()
                    .ClassBody(body + InnerTemplate.GetStaticInnerString(innerClassName, entityClassName))
                    .GetType();

            return TypeCreatorMapping[type] = (Func<DynamicBase>)CtorBuilder.NewDelegate(tempClass);
        }
    }
}
