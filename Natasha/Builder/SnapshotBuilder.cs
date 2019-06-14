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
                        HashSet<{info.TypeName}> compareNew = new HashSet<{info.TypeName}>({NewInstance});
                        HashSet<{info.TypeName}> compareOld = new HashSet<{info.TypeName}>({OldInstance});
                        compareNew.ExceptWith(compareOld);
                        result.Add(""different"",new DiffModel(){{ Name=""different"",Value=compareNew}});
                    }}
                }}
                 return result;
            ");

            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.ComplierOption.UseFileComplie();
            tempBuilder.Using(info.RealType).Using(info.Type).Using(typeof(HashSet<>)); ;
            SnapshotCache[info.RealType] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaSnapshot" + info.AvailableName)
                        .MethodName("Compare")
                        .Param(info.RealType, OldInstance)
                        .Param(info.RealType, NewInstance)
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
                        HashSet<{info.TypeName}> compareOld = new HashSet<{info.TypeName}>({OldInstance});
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
            tempBuilder.Using(info.RealType).Using(info.Type).Using(typeof(HashSet<>)); ;
            SnapshotCache[info.RealType] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaSnapshot" + info.AvailableName)
                        .MethodName("Compare")
                        .Param(info.RealType, OldInstance)
                        .Param(info.RealType, NewInstance)
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
            MethodHandler.Using(info.Type);
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
            MethodHandler.Using(info.Type);
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
            MethodHandler.Using(info.Type);
            Script.Append(
                $@"if({NewInstance}.{info.MemberName}!={OldInstance}.{info.MemberName}){{
                    if({NewInstance}.{info.MemberName}==null || {OldInstance}.{info.MemberName}==null){{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={OldInstance}.{info.MemberName}}});
                    }}else{{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value=NatashaSnapshot{info.AvailableName}.Compare({NewInstance}.{info.MemberName},{OldInstance}.{info.MemberName})}});
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
            tempBuilder.Using(info.Type).Using(typeof(HashSet<>)); ;
            SnapshotCache[info.Type] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaSnapshot" + info.AvailableName)
                        .MethodName("Compare")
                        .Param(info.Type, OldInstance)
                        .Param(info.Type, NewInstance)
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
            tempBuilder.Using(info.Type).Using(typeof(HashSet<>)); ;
            SnapshotCache[info.Type] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaSnapshot" + info.AvailableName)
                        .MethodName("Compare")
                        .Param(info.Type, OldInstance)
                        .Param(info.Type, NewInstance)
                        .MethodBody(scriptBuilder.ToString())          
                        .Return<Dictionary<string, DiffModel>>()             
                        .Complie();
        }




        public override void MemberArrayEntityHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.RealType);
            MethodHandler.Using(info.Type);
            MethodHandler.Using(typeof(HashSet<>));
            Script.Append(
                $@"if({NewInstance}.{info.MemberName}!={OldInstance}.{info.MemberName}){{
                    if({NewInstance}.{info.MemberName}==null || {OldInstance}.{info.MemberName}==null){{
                        result.Add(""{info.MemberName}"",new DiffModel(){{ Name=""{info.MemberName}"",Value={OldInstance}.{info.MemberName}}});
                    }}else{{
                        HashSet<{info.TypeName}> compareOld = new HashSet<{info.TypeName}>({OldInstance}.{info.MemberName});
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
                        HashSet<{info.TypeName}> compareNew = new HashSet<{info.TypeName}>({NewInstance}.{info.MemberName});
                        HashSet<{info.TypeName}> compareOld = new HashSet<{info.TypeName}>({OldInstance}.{info.MemberName});
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
                        .ClassName("NatashaSnapshot" + AvailableNameReverser.GetName(CurrentType))
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
