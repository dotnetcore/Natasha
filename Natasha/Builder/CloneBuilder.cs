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



        public CloneBuilder(Type type=null) {
            CurrentType = type;
            Script = new StringBuilder();
            MethodHandler = new FastMethodOperator();
            IncludeStatic = false;
            IncludeCanRead = true;
            IncludeCanWrite = false;
            //CloneCache[type] = null;
        }




        public override void EntityHandler(Type type)
        {
            MethodHandler.Using("Natasha");
            CloneBuilder builder = new CloneBuilder(type);
            builder.Create();
        }




        public override void OnceTypeHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            //普通类型复制
            scriptBuilder.Append($@"return oldInstance;");

            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.ComplierOption.UseFileComplie();
            tempBuilder.Using(info.DeclaringType);
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())            //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();
        }




        public override void ArrayOnceTypeHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append(@"if(oldInstance!=null){");
            scriptBuilder.Append($"var newInstance = new {info.ElementTypeName}[oldInstance.Length];");
            //普通类型复制
            scriptBuilder.Append(
                $@"for (int i = 0; i < oldInstance.Length; i++){{
                    newInstance[i] = oldInstance[i];
                 }}return newInstance;}}return null;");

            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.ComplierOption.UseFileComplie();
            tempBuilder.Using(info.DeclaringTypeName);
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();
        }





        public override void ArrayEntityHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append(@"if(oldInstance!=null){");
            scriptBuilder.Append($"var newInstance = new {info.ElementTypeName}[oldInstance.Length];");
            //普通类型复制
            scriptBuilder.Append(
                $@"for (int i = 0; i < oldInstance.Length; i+=1){{
                    newInstance[i] =  NatashaClone{info.ElementTypeAvailableName}.Clone(oldInstance[i]);
                 }}return newInstance;}}return null;");

            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.ComplierOption.UseFileComplie();
            tempBuilder.Using(info.DeclaringType);
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .ClassName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();
        }




        public override void CollectionHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append(@"if(oldInstance!=null){");
            var type = info.DeclaringType.GetGenericArguments()[0];
            if (type.IsOnceType())
            {
                scriptBuilder.Append($"return new {info.DeclaringTypeName}(oldInstance);");
            }
            else
            {
                EntityHandler(type);
                scriptBuilder.Append($"return new {info.DeclaringTypeName}(oldInstance.Select(item => NatashaClone{type.GetAvailableName()}.Clone(item)));");
            }
            scriptBuilder.Append("}return null;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Using(info.DeclaringType.GetGenericArguments());
            tempBuilder.Using("Natasha");
            tempBuilder.Using("System.Linq");
            tempBuilder.ComplierOption.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using(info.DeclaringType)
                        .Using(info.DeclaringType.GetAllGenericTypes())
                        .ClassName("NatashaClone" +info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();
        }




        public override void ICollectionHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append(@"if(oldInstance!=null){");
            var type = info.DeclaringType.GetGenericArguments()[0];
            if (type.IsOnceType())
            {
                scriptBuilder.Append($"return new List<{type.GetDevelopName()}>(oldInstance);");
            }
            else
            {
                EntityHandler(type);
                scriptBuilder.Append($"return oldInstance.Select(item => NatashaClone{type.GetAvailableName()}.Clone(item));");
            }
            scriptBuilder.Append("}return null;");

            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Using(info.DeclaringType.GetGenericArguments());
            tempBuilder.Using("Natasha");
            tempBuilder.Using("System.Linq");
            tempBuilder.ComplierOption.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using(info.DeclaringType)
                        .Using(info.DeclaringType.GetAllGenericTypes())
                        .ClassName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();
        }




        public override void DictionaryHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append(@"if(oldInstance!=null){");
            var keyType = info.DeclaringType.GetGenericArguments()[0];
            var valueType = info.DeclaringType.GetGenericArguments()[1];
            scriptBuilder.Append($"return new {info.DeclaringTypeName}(oldInstance.Select(item=>{{return KeyValuePair.Create(");
            if (keyType.IsOnceType())
            {
                scriptBuilder.Append($"item.Key,");
            }
            else
            {
                EntityHandler(keyType);
                scriptBuilder.Append($"NatashaClone{keyType.GetAvailableName()}.Clone(item.Key),");
            }
            if (valueType.IsOnceType())
            {
                scriptBuilder.Append($"item.Value");
            }
            else
            {
                EntityHandler(valueType);
                scriptBuilder.Append($"NatashaClone{valueType.GetAvailableName()}.Clone(item.Value)");
            }
            scriptBuilder.Append(");}));}return null;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Using(info.DeclaringType.GetGenericArguments());
            tempBuilder.Using("Natasha");
            tempBuilder.Using("System.Linq");
            tempBuilder.ComplierOption.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using(info.DeclaringType)
                        .Using(info.DeclaringType.GetAllGenericTypes())
                        .ClassName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                     //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();
        }




        public override void MemberDictionaryHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.MemberType);
            MethodHandler.Using(info.MemberType.GetAllGenericTypes());
            Script.Append($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.Append($@"{NewInstance}.{info.MemberName} = NatashaClone{info.MemberTypeAvailableName}.Clone({OldInstance}.{info.MemberName});}}");
        }




        public override void MemberIDictionaryHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.MemberType);
            MethodHandler.Using(info.MemberType.GetAllGenericTypes());
            Script.Append($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.Append($@"{NewInstance}.{info.MemberName} = NatashaClone{info.MemberTypeAvailableName}.Clone({OldInstance}.{info.MemberName});}}");
        }




        public override void IDictionaryHandler(BuilderInfo info)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.Append(@"if(oldInstance!=null){");
            var keyType = info.DeclaringType.GetGenericArguments()[0];
            var valueType = info.DeclaringType.GetGenericArguments()[1];
            scriptBuilder.Append($"return oldInstance.Select(item=>{{return KeyValuePair.Create(");
            if (keyType.IsOnceType())
            {
                scriptBuilder.Append($"item.Key,");
            }
            else
            {
                EntityHandler(keyType);
                scriptBuilder.Append($"NatashaClone{keyType.GetAvailableName()}.Clone(item.Key),");
            }
            if (valueType.IsOnceType())
            {
                scriptBuilder.Append($"item.Value");
            }
            else
            {
                EntityHandler(valueType);
                scriptBuilder.Append($"NatashaClone{valueType.GetAvailableName()}.Clone(item.Value)");
            }
            scriptBuilder.Append(")});}return null;");


            //创建委托
            var tempBuilder = FastMethodOperator.New;
            tempBuilder.Using(info.DeclaringType.GetGenericArguments());
            tempBuilder.Using("Natasha");
            tempBuilder.Using("System.Linq");
            tempBuilder.ComplierOption.UseFileComplie();
            CloneCache[info.DeclaringType] = tempBuilder
                        .Using("Natasha")
                        .Using(info.DeclaringType)
                        .Using(info.DeclaringType.GetAllGenericTypes())
                        .ClassName("NatashaClone" + info.DeclaringAvailableName)
                        .MethodName("Clone")
                        .Param(info.DeclaringType, "oldInstance")                 //参数
                        .MethodBody(scriptBuilder.ToString())                //方法体
                        .Return(info.DeclaringType)                               //返回类型
                        .Complie();
        }




        public override void EntityStartHandler(BuilderInfo info)
        {
            Script.Append($"if({OldInstance}==null){{return null;}}");
            Script.Append($"{info.DeclaringTypeName} {NewInstance} = new {info.DeclaringTypeName}();");
        }




        public override void EntityReturnHandler(BuilderInfo info)
        {
            Script.Append($"return {NewInstance};");
        }




        public override void MemberOnceTypeHandler(BuilderInfo info)
        {
            Script.Append($"{NewInstance}.{info.MemberName} = {OldInstance}.{info.MemberName};");
        }




        public override void MemberArrayOnceTypeHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.ElementType);
            Script.Append($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.Append($"{NewInstance}.{info.MemberName} = new {info.ElementTypeName}[{OldInstance}.{info.MemberName}.Length];");
            //普通类型复制
            Script.Append(
                $@"for (int i = 0; i < {OldInstance}.{info.MemberName}.Length; i++){{
                      {NewInstance}.{info.MemberName}[i] = {OldInstance}.{info.MemberName}[i];
                }}}}");
        }




        public override void MemberArrayEntityHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.MemberType);
            Script.Append($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.Append($"{NewInstance}.{info.MemberName} = new {info.ElementTypeName}[{OldInstance}.{info.MemberName}.Length];");
            //普通类型复制
            Script.Append(
                $@"for (int i = 0; i < {OldInstance}.{info.MemberName}.Length; i++){{
                      {NewInstance}.{info.MemberName}[i] = NatashaClone{info.ElementTypeAvailableName}.Clone({OldInstance}.{info.MemberName}[i]);
                }}}}");
        }




        public override void MemberICollectionHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.MemberType);
            MethodHandler.Using(info.MemberType.GetAllGenericTypes());
            Script.Append($@"if({OldInstance}.{info.MemberName}!=null){{");
            Script.Append($@"{NewInstance}.{info.MemberName} = NatashaClone{info.MemberTypeAvailableName}.Clone({OldInstance}.{info.MemberName});}}");
        }




        public override void MemberCollectionHandler(BuilderInfo info)
        {
            MethodHandler.Using(info.MemberType);
            MethodHandler.Using(info.MemberType.GetAllGenericTypes());
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
                MethodHandler.ComplierOption.UseFileComplie();
                var @delegate = MethodHandler
                            .ClassName("NatashaClone" + CurrentType.GetAvailableName())
                            .MethodName("Clone")
                            .Param(CurrentType, OldInstance)                //参数
                            .MethodBody(Script.ToString())                 //方法体
                            .Return(CurrentType)                              //返回类型
                           .Complie();
                return CloneCache[CurrentType] = @delegate;
            }
            return null;
        }
    }
}
