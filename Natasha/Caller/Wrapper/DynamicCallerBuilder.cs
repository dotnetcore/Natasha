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
            }

            var props = type.GetProperties();
            for (int i = 0; i < props.Length; i += 1)
            {
                if (!memberCache.ContainsKey(props[i].PropertyType))
                {
                    memberCache[props[i].PropertyType] = new List<string>();
                }
                memberCache[props[i].PropertyType].Add(props[i].Name);
            }





            body.AppendLine("public override T Get<T>(string name){");
            body.Append("var targetType = typeof(T);");
            int indexType = 0;
            foreach (var item in memberCache)
            {
                if (indexType != 0)
                {
                    body.Append("else ");
                }
                body.Append($"if(targetType==typeof({item.Key.GetDevelopName()})){{");
                int indexName = 0;
                foreach (var name in item.Value)
                {
                    if (indexName != 0)
                    {
                        body.Append("else ");
                    }
                    body.Append($"if( name == \"{name}\"){{");
                    body.Append($"return (T)((object)Instance.{name});");
                    body.Append("}");
                    indexName++;
                }
                body.Append("}");
                indexType++;
            }
            body.Append("return default;}");


            body.AppendLine("public override T Get<T>(){");
            body.Append("var targetType = typeof(T);");
            indexType = 0;
            foreach (var item in memberCache)
            {
                if (indexType != 0)
                {
                    body.Append("else ");
                }
                body.Append($"if(targetType==typeof({item.Key.GetDevelopName()})){{");
                int indexName = 0;
                foreach (var name in item.Value)
                {
                    if (indexName != 0)
                    {
                        body.Append("else ");
                    }
                    body.Append($"if( _current_name == \"{name}\"){{");
                    body.Append($"return (T)((object)Instance.{name});");
                    body.Append("}");
                    indexName++;
                }
                body.Append("}");
                indexType++;
            }
            body.Append("return default;}");


            body.AppendLine("public override void Set(string name,object value){");

            foreach (var item in memberCache)
            {
                int indexName = 0;
                foreach (var name in item.Value)
                {
                    if (indexName != 0)
                    {
                        body.Append("else ");
                    }
                    body.Append($"if( name == \"{name}\"){{");
                    body.Append($"Instance.{name}=({item.Key.GetDevelopName()})value;");
                    body.Append("}");
                    indexName++;
                }
            }
            body.Append("}");

            body.AppendLine("public override void Set(object value){");
            foreach (var item in memberCache)
            {
                int indexName = 0;
                foreach (var name in item.Value)
                {
                    if (indexName != 0)
                    {
                        body.Append("else ");
                    }
                    body.Append($"if( _current_name == \"{name}\"){{");
                    body.Append($"Instance.{name}=({item.Key.GetDevelopName()})value;");
                    body.Append("}");
                    indexName++;
                }
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


            body.AppendLine("public override DynamicBase Get(string name){");
            
            foreach (var item in memberCache)
            {
                int indexName = 0;
                if (!item.Key.IsOnceType())
                {
                    foreach (var name in item.Value)
                    {
                        if (indexName != 0)
                        {
                            body.Append("else ");
                        }
                        body.Append($"if( name == \"{name}\"){{");
                        body.Append($"   return Instance.{name}.Caller();");
                        body.Append("}");
                        indexName++;
                    }
                }
            }
            body.Append("return null;}");


            Type tempClass = builder
                    .Using(type)
                    .Using("System")
                    .Using("Natasha.Caller")
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

