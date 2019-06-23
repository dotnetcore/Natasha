namespace Natasha
{
    public class InnerTemplate
    {
        public static string GetNormalInnerString(string type,string className)
        {
            return $@"internal static class {type}<T>{{


                public readonly static ConcurrentDictionary<string,Action<{className},T>> SetterMapping;
                public readonly static ConcurrentDictionary<string,Func<{className},T>> GetterMapping;
                public readonly static ConcurrentDictionary<string,Func<{className},DynamicBase>> LinkMapping;


                static {type}(){{


                    SetterMapping = new ConcurrentDictionary<string,Action<{className},T>>();
                    GetterMapping = new ConcurrentDictionary<string,Func<{className},T>>();
                    LinkMapping = new ConcurrentDictionary<string,Func<{className},DynamicBase>>();
                    

                    Type type = typeof({className});
                    Type memberType = typeof(T);


                    var fields = type.GetFields();
                    for (int i = 0; i < fields.Length; i+=1)
                    {{
                        if (fields[i].FieldType==memberType)
                        {{

                            SetterMapping[fields[i].Name] = FastMethodOperator
                                .New
                                .Using(type)
                                .Param<{className}>(""obj"")
                                .Param<T>(""value"")
                                .MethodBody($@""obj.{{fields[i].Name}} = value;"")
                                .Return()
                                .Complie<Action<{className}, T>>();


                            GetterMapping[fields[i].Name] = FastMethodOperator
                                .New
                                .Using(type)
                                .Param<{className}>(""obj"")
                                .MethodBody($@""return obj.{{fields[i].Name}};"")
                                .Return<T>()
                                .Complie<Func<{className}, T>>();
                        }}
                    }}

                    var props = type.GetProperties(); 
                    for (int i = 0; i < props.Length; i += 1)
                    {{
                        if (props[i].PropertyType == memberType)
                        {{

                            SetterMapping[props[i].Name] = FastMethodOperator
                                .New
                                .Using(type)
                                .Param<{className}>(""obj"")
                                .Param<T>(""value"")
                                .MethodBody($@""obj.{{props[i].Name}} = value;"")
                                .Return()
                                .Complie<Action<{className}, T>>();


                            GetterMapping[props[i].Name] = FastMethodOperator
                                .New
                                .Using(type)
                                .Param<{className}>(""obj"")
                                .MethodBody($@""return obj.{{props[i].Name}};"")
                                .Return<T>()
                                .Complie<Func<{className}, T>>();
                        }}
                    }}
                }}
            }}";
        }


        public static string GetStaticInnerString(string type, string className)
        {
            return $@"internal static class {type}<T>{{


                public readonly static ConcurrentDictionary<string,Action<T>> SetterMapping;
                public readonly static ConcurrentDictionary<string,Func<T>> GetterMapping;
                

                static {type}(){{


                    SetterMapping = new ConcurrentDictionary<string,Action<T>>();
                    GetterMapping = new ConcurrentDictionary<string,Func<T>>();
                    

                    Type type = typeof({className});
                    Type memberType = typeof(T);


                    var fields = type.GetFields();
                    for (int i = 0; i < fields.Length; i+=1)
                    {{
                        if (fields[i].FieldType==memberType)
                        {{

                            SetterMapping[fields[i].Name] = FastMethodOperator
                                .New
                                .Using(type)
                                .Param<T>(""value"")
                                .MethodBody($@""{className}.{{fields[i].Name}} = value;"")
                                .Return()
                                .Complie<Action<T>>();


                            GetterMapping[fields[i].Name] = FastMethodOperator
                                .New
                                .Using(type)
                                .MethodBody($@""return {className}.{{fields[i].Name}};"")
                                .Return<T>()
                                .Complie<Func<T>>();
                        }}
                    }}

                    var props = type.GetProperties(); 
                    for (int i = 0; i < props.Length; i += 1)
                    {{
                        if (props[i].PropertyType == memberType)
                        {{

                            SetterMapping[props[i].Name] = FastMethodOperator
                                .New
                                .Using(type)
                                .Param<T>(""value"")
                                .MethodBody($@""{className}.{{props[i].Name}} = value;"")
                                .Return()
                                .Complie<Action<T>>();


                            GetterMapping[props[i].Name] = FastMethodOperator
                                .New
                                .Using(type)
                                .MethodBody($@""return {className}.{{props[i].Name}};"")
                                .Return<T>()
                                .Complie<Func<T>>();
                        }}
                    }}
                }}
            }}";
        }
    }
}
