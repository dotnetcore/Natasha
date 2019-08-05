using System;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha
{
    public class CloneBuilder<T>
    {

        public static Delegate Create()
        {

            return (new CloneBuilder(typeof(T)).Create());

        }

    }




    public class CloneBuilder : TypeIterator
    {


        public readonly StringBuilder Script;
        private readonly FastMethodOperator MethodHandler;
        private const string NewInstance = "NewInstance";
        private const string OldInstance = "OldInstance";
        public static readonly ConcurrentDictionary<Type, Delegate> CloneCache;


        static CloneBuilder() => CloneCache = new ConcurrentDictionary<Type, Delegate>();




        public CloneBuilder(Type type = null)
        {

            CurrentType = type;
            Script = new StringBuilder();
            MethodHandler = new FastMethodOperator();
            IncludeStatic = false;
            IncludeCanRead = true;
            IncludeCanWrite = false;

        }




        public override void EntityHandler(Type type)
        {

            if (!CloneCache.ContainsKey(type))
            {

                MethodHandler.Using("Natasha");
                CloneBuilder builder = new CloneBuilder(type);
                builder.Create();

            }

        }




        public override void OnceTypeHandler(BuilderInfo info)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append($@"return oldInstance;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Complier.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .OopName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();

        }




        public override void ArrayOnceTypeHandler(BuilderInfo info)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine(@"if(oldInstance!=null){");
            scriptBuilder.AppendLine($"var newInstance = new {info.ElementTypeName}[oldInstance.Length];");
            scriptBuilder.AppendLine(
                $@"for (int i = 0; i < oldInstance.Length; i++){{
                    newInstance[i] = oldInstance[i];
                 }}return newInstance;}}return null;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Complier.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using(info.ElementType)
                        .OopName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();

        }





        public override void ArrayEntityHandler(BuilderInfo info)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine(@"if(oldInstance!=null){");
            scriptBuilder.AppendLine($"var newInstance = new {info.ElementTypeName}[oldInstance.Length];");
            scriptBuilder.Append(
                $@"for (int i = 0; i < oldInstance.Length; i+=1){{
                    newInstance[i] =  NatashaClone{info.ElementTypeAvailableName}.Clone(oldInstance[i]);
                 }}return newInstance;}}return null;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Complier.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using(info.ElementType)
                        .OopName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();

        }




        public override void CollectionHandler(BuilderInfo info)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine(@"if(oldInstance!=null){");


            var type = info.DeclaringType.GetGenericArguments()[0];
            if (type.IsOnceType())
            {

                scriptBuilder.AppendLine($"return new {info.DeclaringTypeName}(oldInstance);");

            }
            else
            {

                EntityHandler(type);
                scriptBuilder.AppendLine($"return new {info.DeclaringTypeName}(oldInstance.Select(item => NatashaClone{type.GetAvailableName()}.Clone(item)));");

            }
            scriptBuilder.AppendLine("}return null;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Complier.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using("System.Linq")
                        .OopName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();
        }




        public override void ICollectionHandler(BuilderInfo info)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine(@"if(oldInstance!=null){");


            var type = info.DeclaringType.GetGenericArguments()[0];
            if (type.IsOnceType())
            {

                scriptBuilder.AppendLine($"return new List<{type.GetDevelopName()}>(oldInstance);");

            }
            else
            {

                EntityHandler(type);
                scriptBuilder.AppendLine($"return oldInstance.Select(item => NatashaClone{type.GetAvailableName()}.Clone(item));");

            }
            scriptBuilder.AppendLine("}return null;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Complier.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using("System.Linq")
                        .OopName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();

        }




        public override void DictionaryHandler(BuilderInfo info)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine(@"if(oldInstance!=null){");
            scriptBuilder.AppendLine($"return new {info.DeclaringTypeName}(oldInstance.Select(item=>{{return KeyValuePair.Create(");


            var keyType = info.DeclaringType.GetGenericArguments()[0];
            var valueType = info.DeclaringType.GetGenericArguments()[1];
            if (keyType.IsOnceType())
            {

                scriptBuilder.Append($"item.Key,");

            }
            else
            {

                EntityHandler(keyType);
                scriptBuilder.AppendLine($"NatashaClone{keyType.GetAvailableName()}.Clone(item.Key),");

            }


            if (valueType.IsOnceType())
            {

                scriptBuilder.Append($"item.Value");

            }
            else
            {

                EntityHandler(valueType);
                scriptBuilder.AppendLine($"NatashaClone{valueType.GetAvailableName()}.Clone(item.Value)");

            }
            scriptBuilder.AppendLine(");}));}return null;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Complier.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using("System.Linq")
                        .OopName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();

        }




        public override void MemberDictionaryHandler(BuilderInfo info)
        {

            MethodHandler.Using(info.MemberType);
            Script.AppendLine($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.AppendLine($@"{NewInstance}.{info.MemberName} = NatashaClone{info.MemberTypeAvailableName}.Clone({OldInstance}.{info.MemberName});}}");

        }




        public override void MemberIDictionaryHandler(BuilderInfo info)
        {

            MethodHandler.Using(info.MemberType);
            Script.AppendLine($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.AppendLine($@"{NewInstance}.{info.MemberName} = NatashaClone{info.MemberTypeAvailableName}.Clone({OldInstance}.{info.MemberName});}}");

        }




        public override void IDictionaryHandler(BuilderInfo info)
        {

            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine(@"if(oldInstance!=null){");
            scriptBuilder.Append($"return oldInstance.Select(item=>{{return KeyValuePair.Create(");


            var keyType = info.DeclaringType.GetGenericArguments()[0];
            var valueType = info.DeclaringType.GetGenericArguments()[1];
            if (keyType.IsOnceType())
            {

                scriptBuilder.Append($"item.Key,");

            }
            else
            {

                EntityHandler(keyType);
                scriptBuilder.AppendLine($"NatashaClone{keyType.GetAvailableName()}.Clone(item.Key),");

            }


            if (valueType.IsOnceType())
            {

                scriptBuilder.Append($"item.Value");

            }
            else
            {

                EntityHandler(valueType);
                scriptBuilder.AppendLine($"NatashaClone{valueType.GetAvailableName()}.Clone(item.Value)");

            }
            scriptBuilder.AppendLine(")});}return null;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Complier.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using("System.Linq")
                        .OopName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();

        }




        public override void EntityStartHandler(BuilderInfo info)
        {

            Script.AppendLine($"if({OldInstance}==null){{return null;}}");
            Script.AppendLine($"{info.DeclaringTypeName} {NewInstance} = new {info.DeclaringTypeName}();");

        }




        public override void EntityReturnHandler(BuilderInfo info)
        {

            Script.AppendLine($"return {NewInstance};");

        }




        public override void MemberOnceTypeHandler(BuilderInfo info)
        {

            Script.AppendLine($"{NewInstance}.{info.MemberName} = {OldInstance}.{info.MemberName};");

        }




        public override void MemberArrayOnceTypeHandler(BuilderInfo info)
        {

            MethodHandler.Using(info.ElementType);


            Script.AppendLine($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.AppendLine($"{NewInstance}.{info.MemberName} = new {info.ElementTypeName}[{OldInstance}.{info.MemberName}.Length];");
            //普通类型复制
            Script.Append(
                $@"for (int i = 0; i < {OldInstance}.{info.MemberName}.Length; i++){{
                      {NewInstance}.{info.MemberName}[i] = {OldInstance}.{info.MemberName}[i];
                }}}}");

        }




        public override void MemberArrayEntityHandler(BuilderInfo info)
        {

            MethodHandler.Using(info.MemberType);
            MethodHandler.Using(info.ElementType);


            Script.AppendLine($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.AppendLine($"{NewInstance}.{info.MemberName} = new {info.ElementTypeName}[{OldInstance}.{info.MemberName}.Length];");
            //普通类型复制
            Script.Append(
                $@"for (int i = 0; i < {OldInstance}.{info.MemberName}.Length; i++){{
                      {NewInstance}.{info.MemberName}[i] = NatashaClone{info.ElementTypeAvailableName}.Clone({OldInstance}.{info.MemberName}[i]);

                }}}}");
        }




        public override void MemberICollectionHandler(BuilderInfo info)
        {

            MethodHandler.Using(info.MemberType);


            Script.AppendLine($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.AppendLine($@"{NewInstance}.{info.MemberName} = NatashaClone{info.MemberTypeAvailableName}.Clone({OldInstance}.{info.MemberName});}}");

        }




        public override void MemberCollectionHandler(BuilderInfo info)
        {

            MethodHandler.Using(info.MemberType);


            Script.Append($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.Append($@"{NewInstance}.{info.MemberName} = NatashaClone{info.MemberTypeAvailableName}.Clone({OldInstance}.{info.MemberName});}}");

        }




        public override void MemberEntityHandler(BuilderInfo info)
        {

            MethodHandler.Using("Natasha");


            Script.Append($"if({OldInstance}.{info.MemberName}!=null){{");
            Script.Append($"{NewInstance}.{info.MemberName} = NatashaClone{info.MemberTypeName}.Clone({OldInstance}.{info.MemberName});}}");

        }




        public Delegate Create()
        {

            if (CloneCache.ContainsKey(CurrentType))
            {

                return CloneCache[CurrentType];

            }


            if (TypeRouter(CurrentType))
            {

                //创建委托
                MethodHandler.Complier.UseFileComplie();
                var @delegate = MethodHandler
                            .OopName("NatashaClone" + CurrentType.GetAvailableName())
                            .MethodName("Clone")
                            .Param(CurrentType, OldInstance)                //参数
                            .MethodBody(Script.ToString())                  //方法体
                            .Return(CurrentType)                            //返回类型
                           .Complie();
                return CloneCache[CurrentType] = @delegate;

            }


            return null;

        }
    }
}
