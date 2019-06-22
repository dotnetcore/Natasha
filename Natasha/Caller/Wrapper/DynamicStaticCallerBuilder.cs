using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

namespace Natasha.Caller.Wrapper
{

    public class DynamicStaticCallerBuilder
    {
        public static readonly ConcurrentDictionary<Type, Func<DynamicBase>> TypeCreatorMapping;
        static DynamicStaticCallerBuilder()
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
            string innerClassName = "InnerDynamicStatic" + type.GetAvailableName();
            string entityClassName = type.GetDevelopName();
          
            ClassBuilder builder = new ClassBuilder();
            StringBuilder body = new StringBuilder();


            body.Append($@" 
                    public override T Get<T>(string name){{
                        return {innerClassName}<T>.GetterMapping[name]();
                    }}");

            body.Append($@" 
                    public override T Get<T>(){{
                        return {innerClassName}<T>.GetterMapping[_current_name]();
                    }}");

            body.Append($@" 
                    public override void Set<T>(string name,T value){{
                        {innerClassName}<T>.SetterMapping[name](value);
                    }}");
            body.Append($@" 
                    public override void Set<T>(T value){{
                        {innerClassName}<T>.SetterMapping[_current_name](value);
                    }}");

            Type tempClass = builder
                    .Using(type)
                    .Using("System")
                    .Using("System.Collections.Concurrent")
                    .ClassAccess(AccessTypes.Public)
                    .ClassName("NatashaDynamicStatic" + type.GetAvailableName())
                    .Namespace("NatashaDynamicStatic")
                    .Inheritance<DynamicBase>()
                    .ClassBody(body + InnerTemplate.GetStaticInnerString(innerClassName, entityClassName))
                    .GetType();

            return TypeCreatorMapping[type] = (Func<DynamicBase>)CtorBuilder.NewDelegate(tempClass);
        }
    }
}
