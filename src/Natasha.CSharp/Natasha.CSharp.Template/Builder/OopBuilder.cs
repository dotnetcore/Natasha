using Natasha.Engine.Utils;
using Natasha.Error;
using Natasha.CSharp.Template;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Natasha.CSharp.Builder
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

        private readonly ConcurrentQueue<IScriptBuilder> _script_cache;
        public CompilationException Exception;
        public OopBuilder()
        {
            _script_cache = new ConcurrentQueue<IScriptBuilder>();
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

            Name(TypeName != default ? TypeName : type.GetDevelopName())
            .Inheritance(type.BaseType)
            .Inheritance(type.GetInterfaces())
            .Namespace(type.Namespace)
           .Access(type)
           .Modifier(type);
            return Link;

        }




        public virtual T Constraint(Action<ConstraintBuilder> constraintAction = null)
        {

            if (constraintAction!=null)
            {
                var instance = new ConstraintBuilder();
                constraintAction.Invoke(instance);
                return Constraint(instance.GetScript());

            }
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
            handler.Name(NameScript);
            action?.Invoke(handler);
            _script_cache.Enqueue(handler);
            return Link;

        }
        public virtual T Ctor(CtorBuilder builder)
        {
            if (builder.NameScript != NameScript)
            {
                builder.Name(NameScript);
            }
            _script_cache.Enqueue(builder);
            return Link;
        }
        public virtual CtorBuilder GetCtorBuilder()
        {
            var builder = new CtorBuilder();
            builder.Name(NameScript);
            _script_cache.Enqueue(builder);
            return builder;
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
            _script_cache.Enqueue(handler);
            return Link;

        }
        public virtual T Method(MethodBuilder builder)
        {

            if (builder != default)
            {
                _script_cache.Enqueue(builder);
            }
            return Link;

        }
        public virtual MethodBuilder GetMethodBuilder()
        {

            var builder = new MethodBuilder();
            _script_cache.Enqueue(builder);
            return builder;

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
            _script_cache.Enqueue(handler);
            return Link;

        }
        public virtual T Field(FieldBuilder builder)
        {

            if (builder != default)
            {
                _script_cache.Enqueue(builder);
            }
            return Link;

        }
        public virtual FieldBuilder GetFieldBuilder()
        {

            var builder = new FieldBuilder();
            _script_cache.Enqueue(builder);
            return builder;

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
            _script_cache.Enqueue(handler);
            return Link;

        }
        public virtual T Property(PropertyBuilder builder)
        {

            if (builder != default)
            {
                _script_cache.Enqueue(builder);
            }
            return Link;

        }
        public virtual PropertyBuilder GetPropertyBuilder()
        {

            var builder = new PropertyBuilder();
            _script_cache.Enqueue(builder);
            return builder;

        }



        //public override 




        public override T BuilderScript()
        {

            foreach (var item in _script_cache)
            {
                UsingRecoder.Union(item.Recoder);
                OnceBodyAppend(item.Script);
            }
            return base.BuilderScript();

        }



        /// <summary>
        /// 精准获取动态类
        /// </summary>
        /// <param name="classIndex">类索引，1开始</param>
        /// <param name="namespaceIndex">命名空间索引，1开始</param>
        /// <returns></returns>
        public virtual Type GetType(OopType oopType, int namespaceIndex = 1, int classIndex = 1)
        {

            Using(AssemblyBuilder.Compiler.Domain.DllAssemblies.Values.ToArray());
            Exception = AssemblyBuilder.Add(this);
            if (!Exception.HasError)
            {
                string name = default;
                switch (oopType)
                {

                    case OopType.Class:

                        name = ScriptHelper.GetClassName(Script, namespaceIndex, classIndex);
                        break;

                    case OopType.Struct:

                        name = ScriptHelper.GetStructName(Script, namespaceIndex, classIndex);
                        break;

                    case OopType.Interface:

                        name = ScriptHelper.GetInterfaceName(Script, namespaceIndex, classIndex);
                        break;

                    case OopType.Enum:

                        name = ScriptHelper.GetEnumName(Script, namespaceIndex, classIndex);
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

            Using(AssemblyBuilder.Compiler.Domain.GetReferenceElements().ToArray());
            Exception = AssemblyBuilder.Add(this);
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
        public T CreateField(string access,string modifier, Type type, string name, string attribute)
        {

            Field(item => item
            .Access(access)
            .Modifier(modifier)
            .Type(type)
            .Name(name)
            .Attribute(attribute)
            );
            Using(type);
            return Link;

        }

        public T CreateField(string access, Type type, string name, string attribute)
        {

            Field(item => item
            .Access(access)
            .Type(type)
            .Name(name)
            .Attribute(attribute)
            );
            Using(type);
            return Link;

        }



        public T PublicReadonlyField<S>(string name, string attribute = default)
        {
            return PublicReadonlyField(typeof(S), name, attribute);
        }
        public T PublicReadonlyField(Type type, string name, string attribute = default)
        {
            return CreateField("public", "readonly", type, name, attribute);
        }

        public T PublicField<S>(string name, string attribute = default)
        {
            return PublicField(typeof(S), name, attribute);
        }
        public T PublicField(Type type, string name, string attribute = default)
        {
            return CreateField("public", type, name, attribute);
        }


        public T PrivateReadonlyField<S>(string name, string attribute = default)
        {
            return PrivateReadonlyField(typeof(S), name, attribute);
        }
        public T PrivateReadonlyField(Type type, string name, string attribute = default)
        {
            return CreateField("private", "readonly", type, name, attribute);
        }

        public T PrivateField<S>(string name, string attribute = default)
        {
            return PrivateField(typeof(S), name, attribute);
        }

        public T PrivateField(Type type, string name, string attribute = default)
        {
            return CreateField("private", type, name, attribute);
        }


        public T InternalReadonlyField<S>(string name, string attribute = default)
        {
            return InternalReadonlyField(typeof(S), name, attribute);
        }
        public T InternalReadonlyField(Type type, string name, string attribute = default)
        {
            return CreateField("internal", "readonly", type, name, attribute);
        }

        public T InternalField<S>(string name, string attribute = default)
        {
            return InternalField(typeof(S), name, attribute);
        }

        public T InternalField(Type type, string name, string attribute = default)
        {
            return CreateField("internal", type, name, attribute);
        }


        public T ProtectedReadonlyField<S>(string name, string attribute = default)
        {
            return ProtectedReadonlyField(typeof(S), name, attribute);
        }
        public T ProtectedReadonlyField(Type type, string name, string attribute = default)
        {
            return CreateField("protected", "readonly", type, name, attribute);
        }

        public T ProtectedField<S>(string name, string attribute = default)
        {
            return ProtectedField(typeof(S), name, attribute);
        }

        public T ProtectedField(Type type, string name, string attribute = default)
        {
            return CreateField("protected", type, name, attribute);
        }

        public T PublicStaticReadonlyField<S>(string name, string attribute = default)
        {
            return PublicStaticReadonlyField(typeof(S), name, attribute);
        }

        public T PublicStaticReadonlyField(Type type, string name, string attribute = default)
        {
            return CreateField("public", "static readonly", type, name, attribute);
        }

        public T PublicStaticField<S>(string name, string attribute = default)
        {
            return PublicStaticField(typeof(S), name, attribute);
        }

        public T PublicStaticField(Type type, string name, string attribute = default)
        {
            return CreateField("public", "static", type, name, attribute);
        }

        public T PrivateStaticReadonlyField<S>(string name, string attribute = default)
        {
            return PrivateStaticReadonlyField(typeof(S), name, attribute);
        }

        public T PrivateStaticReadonlyField(Type type, string name, string attribute = default)
        {
            return CreateField("private", "static readonly", type, name, attribute);
        }

        public T PrivateStaticField<S>(string name, string attribute = default)
        {
            return PrivateStaticField(typeof(S), name, attribute);
        }

        public T PrivateStaticField(Type type, string name, string attribute = default)
        {
            return CreateField("private", "static", type, name, attribute);
        }

        public T InternalStaticReadonlyField<S>(string name, string attribute = default)
        {
            return InternalStaticReadonlyField(typeof(S), name, attribute);
        }

        public T InternalStaticReadonlyField(Type type, string name, string attribute = default)
        {
            return CreateField("internal", "static readonly", type, name, attribute);
        }
        public T InternalStaticField<S>(string name, string attribute = default)
        {
            return InternalStaticField(typeof(S), name, attribute);
        }

        public T InternalStaticField(Type type, string name, string attribute = default)
        {
            return CreateField("internal", "static", type, name, attribute);
        }
        public T ProtectedStaticReadonlyField<S>(string name, string attribute = default)
        {
            return ProtectedStaticReadonlyField(typeof(S), name, attribute);
        }

        public T ProtectedStaticReadonlyField(Type type, string name, string attribute = default)
        {
            return CreateField("protected", "static readonly", type, name, attribute);
        }
        public T ProtectedStaticField<S>(string name, string attribute = default)
        {
            return ProtectedStaticField(typeof(S), name, attribute);
        }

        public T ProtectedStaticField(Type type, string name, string attribute = default)
        {
            return CreateField("protected", "static", type, name, attribute);
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
