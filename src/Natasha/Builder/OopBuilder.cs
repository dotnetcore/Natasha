using Natasha.Core;
using Natasha.Engine.Utils;
using Natasha.Error;
using Natasha.Template;
using System;

namespace Natasha.Builder
{

    public class OopBuilder : OopBuilder<OopBuilder>
    {

        public OopBuilder()
        {
            Link = this;
        }

    }




    public class OopBuilder<T> : UsingTemplate<T> where T: OopBuilder<T>, new()
    {


        public CompilationException Exception;
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

            DefinedName(TypeName != default ? TypeName : type.GetDevelopName())
            .Inheritance(type.BaseType)
            .Inheritance(type.GetInterfaces())
            .Namespace(type.Namespace)
           .Access(type)
           .Modifier(type);
            return Link;

        }





        /// <summary>
        /// 初始化器构建
        /// </summary>
        /// <param name="action">构建委托</param>
        /// <returns></returns>
        public virtual T Ctor(Action<CtorBuilder> action)
        {

            var handler = new CtorBuilder();
            handler.DefinedName(NameScript);
            action?.Invoke(handler);
            RecoderType(handler.UsingRecoder.Types);
            Body(handler.Script);
            return Link;

        }




        /// <summary>
        /// 方法构建
        /// </summary>
        /// <param name="action">方法构建表达式</param>
        /// <returns></returns>
        public virtual T Method(Action<MethodBuilder> action)
        {

            var handler = new MethodBuilder();
            action?.Invoke(handler);
            RecoderType(handler.UsingRecoder.Types);
            Body(handler.Script);
            return Link;

        }




        /// <summary>
        /// 字段构建
        /// </summary>
        /// <param name="action">字段构建表达式</param>
        /// <returns></returns>
        public virtual T Field(Action<FieldBuilder> action)
        {

            var handler = new FieldBuilder();
            action?.Invoke(handler);
            RecoderType(handler.UsingRecoder.Types);
            Body(handler.Script);
            return Link;

        }




        /// <summary>
        /// 属性构建
        /// </summary>
        /// <param name="action">属性构建表达式</param>
        /// <returns></returns>
        public virtual T Property(Action<PropertyBuilder> action)
        {

            var handler = new PropertyBuilder();
            action?.Invoke(handler);
            RecoderType(handler.UsingRecoder.Types);
            Body(handler.Script);
            return Link;

        }




        /// <summary>
        /// 精准获取动态类
        /// </summary>
        /// <param name="classIndex">类索引，1开始</param>
        /// <param name="namespaceIndex">命名空间索引，1开始</param>
        /// <returns></returns>
        public virtual Type GetType(OopType oopType, int classIndex = 1, int namespaceIndex = 1)
        {

            Exception = AssemblyBuilder.Syntax.Add(this);
            if (!Exception.HasError)
            {
                string name = default;
                switch (oopType)
                {

                    case OopType.Class:

                        name = ScriptHelper.GetClassName(Script, classIndex, namespaceIndex);
                        break;

                    case OopType.Struct:

                        name = ScriptHelper.GetStructName(Script, classIndex, namespaceIndex);
                        break;

                    case OopType.Interface:

                        name = ScriptHelper.GetInterfaceName(Script, classIndex, namespaceIndex);
                        break;

                    case OopType.Enum:

                        name = ScriptHelper.GetEnumName(Script, classIndex, namespaceIndex);
                        break;
                }


                var type = AssemblyBuilder.GetTypeFromShortName(name);
                if (type == null)
                {
                    Exception = AssemblyBuilder.Exceptions[0];
                }
                return type;

            }
            return null;
            

        }
        public virtual Type GetType(int classIndex = 1, int namespaceIndex = 1)
        {

            Exception = AssemblyBuilder.Syntax.Add(this);
            if (!Exception.HasError)
            {
                var type = AssemblyBuilder.GetTypeFromShortName(NameScript);
                if (type == null)
                {
                    Exception = AssemblyBuilder.Exceptions[0];
                }
                return type;
            }
            return null;

        }



        /// <summary>
        /// 定义一个字段
        /// </summary>
        /// <param name="access"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T CreateField(string access, Type type, string name, string attribute)
        {

            Field(item => item
            .Access(access)
            .DefinedType(type)
            .DefinedName(name)
            .Attribute(attribute)
            );
            Using(type);
            return Link;

        }

        public T PublicField<S>(string name, string attribute = default)
        {
            return PublicField(typeof(S), name, attribute);
        }

        public T PublicField(Type type, string name, string attribute = default)
        {
            return CreateField("public", type, name, attribute);
        }

        public T PrivateField<S>(string name, string attribute = default)
        {
            return PrivateField(typeof(S), name, attribute);
        }

        public T PrivateField(Type type, string name, string attribute = default)
        {
            return CreateField("private", type, name, attribute);
        }

        public T InternalField<S>(string name, string attribute = default)
        {
            return InternalField(typeof(S), name, attribute);
        }

        public T InternalField(Type type, string name, string attribute = default)
        {
            return CreateField("internal", type, name, attribute);
        }

        public T ProtectedField<S>(string name, string attribute = default)
        {
            return ProtectedField(typeof(S), name, attribute);
        }

        public T ProtectedField(Type type, string name, string attribute = default)
        {
            return CreateField("protected", type, name, attribute);
        }

        public T PublicStaticField<S>(string name, string attribute = default)
        {
            return PublicStaticField(typeof(S), name, attribute);
        }

        public T PublicStaticField(Type type, string name, string attribute = default)
        {
            return CreateField("public static", type, name, attribute);
        }

        public T PrivateStaticField<S>(string name, string attribute = default)
        {
            return PrivateStaticField(typeof(S), name, attribute);
        }

        public T PrivateStaticField(Type type, string name, string attribute = default)
        {
            return CreateField("private static", type, name, attribute);
        }

        public T InternalStaticField<S>(string name, string attribute = default)
        {
            return InternalStaticField(typeof(S), name, attribute);
        }

        public T InternalStaticField(Type type, string name, string attribute = default)
        {
            return CreateField("internal static", type, name, attribute);
        }

        public T ProtectedStaticField<S>(string name, string attribute = default)
        {
            return ProtectedStaticField(typeof(S), name, attribute);
        }

        public T ProtectedStaticField(Type type, string name, string attribute = default)
        {
            return CreateField("protected static", type, name, attribute);
        }

    }


    public enum OopType
    {

        Class,
        Struct,
        Interface,
        Enum

    }

}
