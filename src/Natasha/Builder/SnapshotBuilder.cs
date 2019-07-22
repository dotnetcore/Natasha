using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class SnapshotBuilder : TypeIterator
    {


        public readonly StringBuilder Script;
        private readonly FastMethodOperator MethodHandler;
        private const string NewInstance = "NewInstance";
        private const string OldInstance = "OldInstance";


        public static readonly ConcurrentDictionary<Type, Delegate> SnapshotCache;
        static SnapshotBuilder()=>SnapshotCache = new ConcurrentDictionary<Type, Delegate>();



        public SnapshotBuilder(Type type = null)
        {
            CurrentType = type;
            Script = new StringBuilder();
            MethodHandler = new FastMethodOperator();
        }




        public override void ArrayOnceTypeHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append($@"Dictionary<string,DiffModel> result = new Dictionary<string,DiffModel>();");
            scriptBuilder.Append(
                $@"if({NewInstance}!={OldInstance}){{
                    if({NewInstance}==null || {OldInstance}==null){{
                        result.Add(""different"",new DiffModel(){{ Name=""different"",Value={OldInstance}}});
                    }}else{{
                        HashSet<{info.ElementTypeName}> compareNew = new HashSet<{info.ElementTypeName}>({NewInstance});
                        HashSet<{info.ElementTypeName}> compareOld = new HashSet<{info.ElementTypeName}>({OldInstance});
                        compareNew.ExceptWith(compareOld);
                        result.Add(""different"",new DiffModel(){{ Name=""different"",Value=compareNew}});
                    }}
                }}
                 return result;
            ");

            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.ComplierOption.UseFileComplie();
            tempBuilder.Using(info.ElementType).Using(info.DeclaringType).Using(typeof(HashSet<>)); ;
            SnapshotCache[info.ElementType] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaSnapshot" + info.DeclaringAvailableName)
                        .MethodName("Compare")
                        .Param(info.ElementType, OldInstance)
                        .Param(info.ElementType, NewInstance)
                        .MethodBody(scriptBuilder.ToString())      
                        .Return<Dictionary<string, DiffModel>>()  
                        .Complie();
        }




        public override void ArrayEntityHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append($@"Dictionary<string,DiffModel> result = new Dictionary<string,DiffModel>();");
            scriptBuilder.Append(
                $@"if({NewInstance}!={OldInstance}){{
                    if({NewInstance}==null || {OldInstance}==null){{
                        result.Add(""different"",new DiffModel(){{ Name=""different"",Value={OldInstance}}});
                    }}else{{
                        HashSet<{info.ElementTypeName}> compareOld = new HashSet<{info.ElementTypeName}>({OldInstance});
                        for(int j = 0; j < OldInstance.Length;j+=1){{
                            for(int i = 0; i < NewInstance.Length;i+=1){{
                                if(SnapshotOperator.Diff({NewInstance}[i],{OldInstance}[j]).Count==0){{
                                    compareOld.Remove({OldInstance}[j]);
                                }}
                            }}
                        }}
                        result.Add(""different"",new DiffModel(){{ Name=""different"",Value=compareOld}});
                    }}
                }}
                 return result;
            ");

            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.ComplierOption.UseFileComplie();
            tempBuilder.Using(info.ElementType).Using(info.DeclaringTypeName).Using(typeof(HashSet<>)); ;
            SnapshotCache[info.ElementType] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaSnapshot" + info.DeclaringAvailableName)
                        .MethodName("Compare")
                        .Param(info.ElementType, OldInstance)
                        .Param(info.ElementType, NewInstance)
                        .MethodBody(scriptBuilder.ToString())         
                        .Return<Dictionary<string, DiffModel>>()                  
                        .Complie();
        }




        public override void MemberOnceTypeHandler(BuilderInfo info)
        {
            Script.Append($@"if({NewInstance}.{info.MemberName}!={OldInstance}.{info.MemberName}){{
                    result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={OldInstance}.{info.MemberName} }});
            }}");
        }




        public override void MemberICollectionHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.MemberType);
            Script.Append(
                $@"if({NewInstance}.{info.MemberName}!={OldInstance}.{info.MemberName}){{
                    if({NewInstance}.{info.MemberName}==null || {OldInstance}.{info.MemberName}==null){{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={OldInstance}.{info.MemberName}}});
                    }}else{{
                         result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={NewInstance}.{info.MemberName}.SnapshotExtension({OldInstance}.{info.MemberName})}});
                    }}
                }}   
            ");
        }




        public override void MemberCollectionHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.MemberType);
            Script.Append(
                $@"if({NewInstance}.{info.MemberName}!={OldInstance}.{info.MemberName}){{
                    if({NewInstance}.{info.MemberName}==null || {OldInstance}.{info.MemberName}==null){{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={OldInstance}.{info.MemberName}}});
                    }}else{{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={NewInstance}.{info.MemberName}.SnapshotExtension({OldInstance}.{info.MemberName})}});
                    }}
                }}   
            ");
        }




        public override void MemberEntityHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.MemberType);
            Script.Append(
                $@"if({NewInstance}.{info.MemberName}!={OldInstance}.{info.MemberName}){{
                    if({NewInstance}.{info.MemberName}==null || {OldInstance}.{info.MemberName}==null){{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={OldInstance}.{info.MemberName}}});
                    }}else{{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value=NatashaSnapshot{info.MemberTypeAvailableName}.Compare({NewInstance}.{info.MemberName},{OldInstance}.{info.MemberName})}});
                    }}
                }}   
            ");
        }




        public override void CollectionHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append($@"Dictionary<string,DiffModel> result = new Dictionary<string,DiffModel>();");
            scriptBuilder.Append(
                $@"if({NewInstance}!={OldInstance}){{
                    if({NewInstance}==null || {OldInstance}==null){{
                        result.Add(""different"",new DiffModel(){{ Name=""different"",Value={OldInstance}}});
                    }}else{{
                       
                        result.Add(""different"",new DiffModel(){{ Name=""different"",Value={NewInstance}.SnapshotExtension({OldInstance})}});
                    }}
                }}
                 return result;
            ");

            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.ComplierOption.UseFileComplie();
            tempBuilder.Using(info.DeclaringType).Using(typeof(HashSet<>)); ;
            SnapshotCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaSnapshot" + info.DeclaringAvailableName)
                        .MethodName("Compare")
                        .Param(info.DeclaringType, OldInstance)
                        .Param(info.DeclaringType, NewInstance)
                        .MethodBody(scriptBuilder.ToString())            
                        .Return<Dictionary<string, DiffModel>>()       
                        .Complie();
        }




        public override void ICollectionHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append($@"Dictionary<string,DiffModel> result = new Dictionary<string,DiffModel>();");
            scriptBuilder.Append(
                $@"if({NewInstance}!={OldInstance}){{
                    if({NewInstance}==null || {OldInstance}==null){{
                        result.Add(""different"",new DiffModel(){{ Name=""different"",Value={OldInstance}}});
                    }}else{{
                       
                        result.Add(""different"",new DiffModel(){{ Name=""different"",Value={NewInstance}.SnapshotExtension({OldInstance})}});
                    }}
                }}
                return result;
            ");

            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.ComplierOption.UseFileComplie();
            tempBuilder.Using(info.DeclaringType).Using(typeof(HashSet<>)); ;
            SnapshotCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaSnapshot" + info.DeclaringAvailableName)
                        .MethodName("Compare")
                        .Param(info.DeclaringType, OldInstance)
                        .Param(info.DeclaringType, NewInstance)
                        .MethodBody(scriptBuilder.ToString())          
                        .Return<Dictionary<string, DiffModel>>()             
                        .Complie();
        }




        public override void MemberArrayEntityHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.ElementType);
            MethodHandler.Using(info.MemberType);
            MethodHandler.Using(typeof(HashSet<>));
            Script.Append(
                $@"if({NewInstance}.{info.MemberName}!={OldInstance}.{info.MemberName}){{
                    if({NewInstance}.{info.MemberName}==null || {OldInstance}.{info.MemberName}==null){{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={OldInstance}.{info.MemberName}}});
                    }}else{{
                        HashSet<{info.ElementTypeName}> compareOld = new HashSet<{info.ElementTypeName}>({OldInstance}.{info.MemberName});
                        for(int j = 0; j < {OldInstance}.{info.MemberName}.Length;j+=1){{
                            for(int i = 0; i < {NewInstance}.{info.MemberName}.Length;i+=1){{
                                if(SnapshotOperator.Diff({NewInstance}.{info.MemberName}[i],{OldInstance}.{info.MemberName}[j]).Count==0){{
                                    compareOld.Remove({OldInstance}.{info.MemberName}[j]);
                                }}
                            }}
                        }}
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value=compareOld}});
                    }}
                }}
            ");
        }




        public override void MemberArrayOnceTypeHandler(BuilderInfo info)
        {
            MethodHandler.Using(typeof(HashSet<>));
            Script.Append(
                $@"if({NewInstance}.{info.MemberName}!={OldInstance}.{info.MemberName}){{
                    if({NewInstance}.{info.MemberName}==null || {OldInstance}.{info.MemberName}==null){{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={OldInstance}.{info.MemberName}}});
                    }}else{{
                        HashSet<{info.ElementTypeName}> compareNew = new HashSet<{info.ElementTypeName}>({NewInstance}.{info.MemberName});
                        HashSet<{info.ElementTypeName}> compareOld = new HashSet<{info.ElementTypeName}>({OldInstance}.{info.MemberName});
                        compareOld.ExceptWith(compareNew);
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value=compareOld}});
                    }}
                }}   
            ");
        }




        public override void EntityStartHandler(BuilderInfo info)
        {
            Script.Append($@"Dictionary<string,DiffModel> result = new Dictionary<string,DiffModel>();");
        }




        public override void EntityReturnHandler(BuilderInfo info)
        {
            Script.Append($@"return result;");
        }




        /// <summary>
        /// 根据委托强类型获取强类型
        /// </summary>
        /// <typeparam name="T">强类型</typeparam>
        public static void CreateSnapshotDelegate<T>()
        {
            SnapshotOperator<T>.CompareFunc = (Func<T, T, Dictionary<string, DiffModel>>)((new SnapshotBuilder(typeof(T)).Create()));
        }




        public override void EntityHandler(Type type)
        {
            MethodHandler.Using("Natasha");
            SnapshotBuilder builder = new SnapshotBuilder(type);
            builder.Create();
        }




        public Delegate Create()
        {
            TypeRouter(CurrentType);
            //创建委托
            MethodHandler.ComplierOption.UseFileComplie();
            var @delegate = MethodHandler
                        .ClassName("NatashaSnapshot" + CurrentType.GetAvailableName())
                        .MethodName("Compare")
                        .Param(CurrentType, NewInstance)
                        .Param(CurrentType, OldInstance)
                        .MethodBody(Script.ToString())                 //方法体
                        .Return<Dictionary<string,DiffModel>>()                              //返回类型
                       .Complie();
            return SnapshotCache[CurrentType] = @delegate;
        }
    }
}
