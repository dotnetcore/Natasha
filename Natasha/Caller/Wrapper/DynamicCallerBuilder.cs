using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

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


            body.Append($@" 
                    public override void New(){{
                         Instance = new {type.GetDevelopName()}();
                    }}");

            body.Append($@" 
                    public override T Get<T>(string name){{
                        return {innerBody}<T>.GetterMapping[name](Instance);
                    }}");

            body.Append($@" 
                    public override T Get<T>(){{
                        return {innerBody}<T>.GetterMapping[_current_name](Instance);
                    }}");

            body.Append($@" 
                    public override void Set<T>(string name,T value){{
                        {innerBody}<T>.SetterMapping[name](Instance,value);
                    }}");
            body.Append($@" 
                    public override void Set<T>(T value){{
                        {innerBody}<T>.SetterMapping[_current_name](Instance,value);
                    }}");

            Type tempClass = builder
                    .Using(type)
                    .Using("System")
                    .Using("System.Collections.Concurrent")
                    .ClassAccess(AccessTypes.Public)
                    .ClassName("NatashaDynamic" + type.GetAvailableName())
                    .Namespace("NatashaDynamic")
                    .Inheritance(GenericBuilder.GetType(typeof(DynamicBase<>),type))
                    .ClassBody(body + InnerTemplate.GetNormalInnerString(innerBody, entityClassName))
                    .GetType();

            return TypeCreatorMapping[type] = (Func<DynamicBase>)CtorBuilder.NewDelegate(tempClass);
        }
    }
}
