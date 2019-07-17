using System;
using System.Collections.Concurrent;
using System.Text;
using System.Collections.Generic;

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

            Dictionary<Type, List<string>> memberCache = new Dictionary<Type, List<string>>();

            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i += 1)
            {
                if (!memberCache.ContainsKey(fields[i].FieldType))
                {
                    memberCache[fields[i].FieldType] = new List<string>();
                }
                memberCache[fields[i].FieldType].Add(fields[i].Name);
                body.AppendLine($"private static readonly Func<{type.GetDevelopName()},{fields[i].FieldType.GetDevelopName()}> CallerInnerGetter{fields[i].Name};");
                body.AppendLine($"private static readonly Action<{type.GetDevelopName()},{fields[i].FieldType.GetDevelopName()}> CallerInnerSetter{fields[i].Name};");
            }

            var props = type.GetProperties();
            for (int i = 0; i < props.Length; i += 1)
            {
                if (!memberCache.ContainsKey(props[i].PropertyType))
                {
                    memberCache[props[i].PropertyType] = new List<string>();
                }
                memberCache[props[i].PropertyType].Add(props[i].Name);
                body.AppendLine($"private static readonly Func<{type.GetDevelopName()},{props[i].PropertyType.GetDevelopName()}> CallerInnerGetter{props[i].Name};");
                body.AppendLine($"private static readonly Action<{type.GetDevelopName()},{props[i].PropertyType.GetDevelopName()}> CallerInnerSetter{props[i].Name};");
            }



            body.AppendLine($"static {className}(){{");
            foreach (var item in memberCache)
            {
                foreach (var name in item.Value)
                {
                    body.AppendLine($@"CallerInnerGetter{name} = FastMethodOperator
                                                                .New
                                                                .Using(""Natasha.Caller"")
                                                                .Param <{type.GetDevelopName()}>(""obj"")
                                                                .MethodBody(""return obj.{name};"")
                                                                .Return<{item.Key.GetDevelopName()}>()
                                                                .Complie<Func<{type.GetDevelopName()}, {item.Key.GetDevelopName()}>>();");
                    body.AppendLine($@"CallerInnerSetter{name} = FastMethodOperator
                                                                .New
                                                                .Using(""Natasha.Caller"")
                                                                .Param<{type.GetDevelopName()}>(""obj"")
                                                                .Param<{item.Key.GetDevelopName()}>(""value"")
                                                                .MethodBody(""obj.{name} = value;"")
                                                                .Return()
                                                                .Complie<Action<{type.GetDevelopName()}, {item.Key.GetDevelopName()}>>();");
                }
            }
            body.Append("}");



            body.AppendLine("public override T Get<T>(string name)where T : Int32,String{");

            int indexType = 0;
            foreach (var item in memberCache)
            {
                if (indexType != 0)
                {
                    body.Append("else ");
                }
                body.Append($"if(typeof(T)==typeof({item.Key.GetDevelopName()})){{");
                int indexName = 0;
                foreach (var name in item.Value)
                {
                    if (indexName != 0)
                    {
                        body.Append("else ");
                    }
                    body.Append($"if( name == \"{name}\"){{");
                    body.Append($"return (T)CallerInnerGetter{name}(Instance);");
                    body.Append("}");
                    indexName++;
                }
                body.Append("}");
                indexType++;
            }
            body.Append("return default;}");


            body.AppendLine("public override T Get<T>()where T : Int32,String{");
            indexType = 0;
            foreach (var item in memberCache)
            {
                if (indexType != 0)
                {
                    body.Append("else ");
                }
                body.Append($"if(typeof(T)==typeof({item.Key.GetDevelopName()})){{");
                int indexName = 0;
                foreach (var name in item.Value)
                {
                    if (indexName != 0)
                    {
                        body.Append("else ");
                    }
                    body.Append($"if( _current_name == \"{name}\"){{");
                    body.Append($"return (T)CallerInnerGetter{name}(Instance);");
                    body.Append("}");
                    indexName++;
                }
                body.Append("}");
                indexType++;
            }
            body.Append("return default;}");


            body.AppendLine("public override void Set<T>(string name,T value) where T : Int32,String{");

            indexType = 0;
            foreach (var item in memberCache)
            {
                if (indexType != 0)
                {
                    body.Append("else ");
                }
                int indexName = 0;
                body.Append($"if(typeof(T)==typeof({item.Key.GetDevelopName()})){{");
                foreach (var name in item.Value)
                {
                    if (indexName != 0)
                    {
                        body.Append("else ");
                    }
                    body.Append($"if( name == \"{name}\"){{");
                    body.Append($"CallerInnerSetter{name}(Instance,(T)value);");
                    body.Append("}");
                    indexName++;
                }
                body.Append("}");
                indexType++;
            }
            body.Append("}");

            body.AppendLine("public override void Set<T>(T value) where T : Int32,String{");
            indexType = 0;
            foreach (var item in memberCache)
            {
                if (indexType != 0)
                {
                    body.Append("else ");
                }
                body.Append($"if(typeof(T)==typeof({item.Key.GetDevelopName()})){{");
                int indexName = 0;
                foreach (var name in item.Value)
                {
                    if (indexName != 0)
                    {
                        body.Append("else ");
                    }
                    body.Append($"if( _current_name == \"{name}\"){{");
                    body.Append($"CallerInnerSetter{name}(Instance,(T)value);");
                    body.Append("}");
                    indexName++;
                }
                body.Append("}");
            }
            body.Append("}");




            //body.Append($@" 
            //        public readonly static ConcurrentDictionary<string,Func<{entityClassName},DynamicBase>> LinkMapping;
            //        static {className}(){{

            //            LinkMapping = new ConcurrentDictionary<string,Func<{entityClassName},DynamicBase>>();
                       

            //            var fields = typeof({entityClassName}).GetFields();
            //            for (int i = 0; i < fields.Length; i+=1)
            //            {{
            //                if(!fields[i].FieldType.IsOnceType())
            //                {{
            //                    LinkMapping[fields[i].Name] = FastMethodOperator
            //                                                    .New
            //                                                    .Using(fields[i].FieldType)
            //                                                    .Using(""Natasha.Caller"")
            //                                                    .Param<{entityClassName}>(""obj"")
            //                                                    .MethodBody($@""return obj.{{fields[i].Name}}.Caller<{{fields[i].FieldType.GetDevelopName()}}>();"")
            //                                                    .Return<DynamicBase>()
            //                                                    .Complie<Func<{entityClassName}, DynamicBase>>();
            //                }}
            //            }}


            //            var props = typeof({entityClassName}).GetProperties(); 
            //            for (int i = 0; i < props.Length; i+=1)
            //            {{
            //                if(!props[i].PropertyType.IsOnceType())
            //                {{
            //                    LinkMapping[props[i].Name] = FastMethodOperator
            //                                                    .New
            //                                                    .Using(props[i].PropertyType)
            //                                                    .Using(""Natasha.Caller"")
            //                                                    .Param<{entityClassName}>(""obj"")
            //                                                    .MethodBody($@""return obj.{{props[i].Name}}.Caller<{{props[i].PropertyType.GetDevelopName()}}>();"")
            //                                                    .Return<DynamicBase>()
            //                                                    .Complie<Func<{entityClassName}, DynamicBase>>();
            //                }}
            //            }}
            //        }}");

            body.Append($@" 
                    public override void New(){{
                         Instance = new {type.GetDevelopName()}();
                    }}");

            //body.Append($@" 
            //        public override DynamicBase Get(string name){{
            //             return LinkMapping[name](Instance);
            //        }}");



            Type tempClass = builder
                    .Using(type)
                    .Using("System")
                    .Using("System.Collections.Concurrent")
                    .ClassAccess(AccessTypes.Public)
                    .ClassName(className)
                    .Namespace("NatashaDynamic")
                    .Inheritance(typeof(DynamicBase<>).With(type))
                    .ClassBody(body)
                    .GetType();

            return TypeCreatorMapping[type] = (Func<DynamicBase>)CtorBuilder.NewDelegate(tempClass);
        }
    }
}
