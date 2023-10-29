using Natasha.CSharp.Extension.Inner;
using Natasha.CSharp.Template;
using System;
using System.Runtime.CompilerServices;

namespace Natasha.CSharp.Builder
{


    public class OopBuilder : OopBuilder<OopBuilder>
    {

        public OopBuilder()
        {
            Link = this;
        }

    }



    public partial class OopBuilder<T> : UsingTemplate<T> where T : OopBuilder<T>, new()
    {

        //rivate readonly ConcurrentQueue<Script> _script_cache;
        public NatashaException? Exception;
        public OopBuilder()
        {
            UseRandomName();
        }
        /// <summary>
        /// 指定外部类型和类型名来仿制构建结构的命名空间，保护级别，特殊修饰符，继承的结构，结构名
        /// </summary>
        /// <param name="type">外部类型</param>
        /// <param name="TypeName">结构名称</param>
        /// <returns></returns>
        public T UseType(Type type, string TypeName)
        {

            if (type.IsEnum)
            {

                this.Enum();

            }
            else if (type.IsInterface)
            {

                this.Interface();

            }
            else if (type.IsValueType)
            {

                this.Struct();

            }
            else
            {

                this.Class();

            }
            Name(TypeName != string.Empty ? TypeName : type.GetDevelopName())
            .InheritanceAppend(type.BaseType)
            .InheritanceAppend(type.GetInterfaces())
            .Namespace(type.Namespace)
           .Access(type)
           .Modifier(type);
            return Link;

        }


        public T SkipInit()
        {
            this.AssemblyBuilder.ConfigCompilerOption(item => item.RemoveIgnoreAccessibility());
            this.AttributeAppend<SkipLocalsInitAttribute>();
            return Link;
        }




        public virtual T Constraint(Action<ConstraintBuilder> constraintAction)
        {

            var instance = new ConstraintBuilder();
            constraintAction.Invoke(instance);
            return Constraint(instance.GetScript());

        }




        /// <summary>
        /// 初始化器构建
        /// </summary>
        /// <param name="action">构建委托</param>
        /// <returns></returns>
        public virtual T Ctor(Action<CtorBuilder> action)
        {

            var handler = new CtorBuilder();
            handler.Name(NameScript);
            action.Invoke(handler);
            RecordUsing(handler.UsingRecorder);
            BodyAppend(handler.GetScript());
            return Link;

        }
        public virtual T Ctor(CtorBuilder builder)
        {
            if (builder.NameScript != NameScript)
            {
                builder.Name(NameScript);
            }
            RecordUsing(builder.UsingRecorder);
            BodyAppend(builder.GetScript());
            return Link;
        }



        /// <summary>
        /// 方法构建
        /// </summary>
        /// <param name="action">方法构建表达式</param>
        /// <returns></returns>
        public virtual T Method(Action<MethodBuilder> action)
        {

            var builder = new MethodBuilder();
            action.Invoke(builder);
            RecordUsing(builder.UsingRecorder);
            BodyAppend(builder.GetScript());
            return Link;

        }
        public virtual T Method(MethodBuilder builder)
        {
            RecordUsing(builder.UsingRecorder);
            BodyAppend(builder.GetScript());
            return Link;

        }



        /// <summary>
        /// 字段构建
        /// </summary>
        /// <param name="action">字段构建表达式</param>
        /// <returns></returns>
        public virtual T Field(Action<FieldBuilder> action)
        {

            var builder = new FieldBuilder();
            action.Invoke(builder);
            RecordUsing(builder.UsingRecorder);
            BodyAppend(builder.GetScript());
            return Link;

        }
        public virtual T Field(FieldBuilder builder)
        {

            RecordUsing(builder.UsingRecorder);
            BodyAppend(builder.GetScript());
            return Link;

        }



        /// <summary>
        /// 属性构建
        /// </summary>
        /// <param name="action">属性构建表达式</param>
        /// <returns></returns>
        public virtual T Property(Action<PropertyBuilder> action)
        {

            var builder = new PropertyBuilder();
            action.Invoke(builder);
            RecordUsing(builder.UsingRecorder);
            BodyAppend(builder.GetScript());
            return Link;

        }
        public virtual T Property(PropertyBuilder builder)
        {

            RecordUsing(builder.UsingRecorder);
            BodyAppend(builder.GetScript());
            return Link;

        }

        /// <summary>
        /// 精准获取动态类
        /// </summary>
        /// <param name="oopIndex">类索引，0开始</param>
        /// <param name="namespaceIndex">命名空间索引，0开始</param>
        /// <returns></returns>
        public virtual Type GetType(OopType oopType, int namespaceIndex = 0, int oopIndex = 0)
        {

            BuildTree();
            string? name = default;
            switch (oopType)
            {

                case OopType.Class:

                    name = AssemblyBuilder.SyntaxTrees[0].NamespaceNode(namespaceIndex).GetClassName(oopIndex);
                    break;

                case OopType.Struct:

                    name = AssemblyBuilder.SyntaxTrees[0].NamespaceNode(namespaceIndex).GetStructName(oopIndex);
                    break;

                case OopType.Interface:

                    name = AssemblyBuilder.SyntaxTrees[0].NamespaceNode(namespaceIndex).GetInterfaceName(oopIndex);
                    break;

                case OopType.Enum:

                    name = AssemblyBuilder.SyntaxTrees[0].NamespaceNode(namespaceIndex).GetEnumName(oopIndex);
                    break;

                case OopType.Record:

                    name = AssemblyBuilder.SyntaxTrees[0].NamespaceNode(namespaceIndex).GetRecordName(oopIndex);
                    break;
            }
            var assembly = AssemblyBuilder.GetAssembly();
            return assembly.GetTypeFromShortName(name!);

        }

        public new virtual Type GetType()
        {

            BuildTree();
            var assembly = AssemblyBuilder.GetAssembly();
            return assembly.GetTypeFromShortName(NameScript);

        }



        /// <summary>
        /// 定义一个字段
        /// </summary>
        /// <param name="access"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T CreateField(string access, string modifier, Type type, string name, string? attribute)
        {

            Field(item =>
            {
                item
                 .Access(access)
                 .Modifier(modifier)
                 .Type(type)
                 .Name(name);
                if (!string.IsNullOrEmpty(attribute))
                {
                    item.AttributeAppend(attribute);
                }

            });
            Using(type);
            return Link;

        }

        public T CreateField(string access, Type type, string name, string? attribute)
        {

            Field(item =>
            {
                item
                 .Access(access)
                 .Type(type)
                 .Name(name);
                if (!string.IsNullOrEmpty(attribute))
                {
                    item.AttributeAppend(attribute);
                }
            });
            Using(type);
            return Link;

        }



        public T PublicReadonlyField<S>(string name, string? attribute = default)
        {
            return PublicReadonlyField(typeof(S), name, attribute);
        }
        public T PublicReadonlyField(Type type, string name, string? attribute = default)
        {
            return CreateField("public", "readonly", type, name, attribute);
        }

        public T PublicField<S>(string name, string? attribute = default)
        {
            return PublicField(typeof(S), name, attribute);
        }
        public T PublicField(Type type, string name, string? attribute = default)
        {
            return CreateField("public", type, name, attribute);
        }


        public T PrivateReadonlyField<S>(string name, string? attribute = default)
        {
            return PrivateReadonlyField(typeof(S), name, attribute);
        }
        public T PrivateReadonlyField(Type type, string name, string? attribute = default)
        {
            return CreateField("private", "readonly", type, name, attribute);
        }

        public T PrivateField<S>(string name, string? attribute = default)
        {
            return PrivateField(typeof(S), name, attribute);
        }

        public T PrivateField(Type type, string name, string? attribute = default)
        {
            return CreateField("private", type, name, attribute);
        }


        public T InternalReadonlyField<S>(string name, string? attribute = default)
        {
            return InternalReadonlyField(typeof(S), name, attribute);
        }
        public T InternalReadonlyField(Type type, string name, string? attribute = default)
        {
            return CreateField("internal", "readonly", type, name, attribute);
        }

        public T InternalField<S>(string name, string? attribute = default)
        {
            return InternalField(typeof(S), name, attribute);
        }

        public T InternalField(Type type, string name, string? attribute = default)
        {
            return CreateField("internal", type, name, attribute);
        }


        public T ProtectedReadonlyField<S>(string name, string? attribute = default)
        {
            return ProtectedReadonlyField(typeof(S), name, attribute);
        }
        public T ProtectedReadonlyField(Type type, string name, string? attribute = default)
        {
            return CreateField("protected", "readonly", type, name, attribute);
        }

        public T ProtectedField<S>(string name, string? attribute = default)
        {
            return ProtectedField(typeof(S), name, attribute);
        }

        public T ProtectedField(Type type, string name, string? attribute = default)
        {
            return CreateField("protected", type, name, attribute);
        }

        public T PublicStaticReadonlyField<S>(string name, string? attribute = default)
        {
            return PublicStaticReadonlyField(typeof(S), name, attribute);
        }

        public T PublicStaticReadonlyField(Type type, string name, string? attribute = default)
        {
            return CreateField("public", "static readonly", type, name, attribute);
        }

        public T PublicStaticField<S>(string name, string? attribute = default)
        {
            return PublicStaticField(typeof(S), name, attribute);
        }

        public T PublicStaticField(Type type, string name, string? attribute = default)
        {
            return CreateField("public", "static", type, name, attribute);
        }

        public T PrivateStaticReadonlyField<S>(string name, string? attribute = default)
        {
            return PrivateStaticReadonlyField(typeof(S), name, attribute);
        }

        public T PrivateStaticReadonlyField(Type type, string name, string? attribute = default)
        {
            return CreateField("private", "static readonly", type, name, attribute);
        }

        public T PrivateStaticField<S>(string name, string? attribute = default)
        {
            return PrivateStaticField(typeof(S), name, attribute);
        }

        public T PrivateStaticField(Type type, string name, string? attribute = default)
        {
            return CreateField("private", "static", type, name, attribute);
        }

        public T InternalStaticReadonlyField<S>(string name, string? attribute = default)
        {
            return InternalStaticReadonlyField(typeof(S), name, attribute);
        }

        public T InternalStaticReadonlyField(Type type, string name, string? attribute = default)
        {
            return CreateField("internal", "static readonly", type, name, attribute);
        }
        public T InternalStaticField<S>(string name, string? attribute = default)
        {
            return InternalStaticField(typeof(S), name, attribute);
        }

        public T InternalStaticField(Type type, string name, string? attribute = default)
        {
            return CreateField("internal", "static", type, name, attribute);
        }
        public T ProtectedStaticReadonlyField<S>(string name, string? attribute = default)
        {
            return ProtectedStaticReadonlyField(typeof(S), name, attribute);
        }

        public T ProtectedStaticReadonlyField(Type type, string name, string? attribute = default)
        {
            return CreateField("protected", "static readonly", type, name, attribute);
        }
        public T ProtectedStaticField<S>(string name, string? attribute = default)
        {
            return ProtectedStaticField(typeof(S), name, attribute);
        }

        public T ProtectedStaticField(Type type, string name, string? attribute = default)
        {
            return CreateField("protected", "static", type, name, attribute);
        }


        /// <summary>
        /// 只读包装属性
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T RecordProperty<S>(string name)
        {
            return RecordProperty(typeof(S), name);
        }
        /// <summary>
        /// 只读包装属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T RecordProperty(Type type, string name)
        {
            return Property(item => item
            .Public()
            .Name(name)
            .Type(type)
            .Getter()
            .InitSetter());
        }
    }


    public enum OopType
    {

        Class,
        Struct,
        Interface,
        Enum,
        Record

    }

}
