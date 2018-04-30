using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.Reflection
{
    public class Reflector
    {
        public static BindingFlags Flag;
        public Type PacketType;
        public Type RealType;

        public bool IsNestedPrivate;

        public Reflector(Type type)
        {
            if (type.IsArray)
            {
                PacketType = type;
                RealType = PacketType.GetElementType();
            }
            else
            {
                RealType = type;
            }

            if (RealType.GetTypeInfo().IsNestedPrivate)
            {
                IsNestedPrivate = true;
            }
        }



        static Reflector()
        {
            Flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        }

        public FieldInfo Field(string name)
        {
            return RealType.GetField(name, Flag);
        }
        public FieldInfo[] Field()
        {
            return RealType.GetFields(Flag);
        }
        public PropertyInfo Property(string name)
        {
            return RealType.GetProperty(name, Flag);
        }
        public PropertyInfo[] Properties()
        {
            return RealType.GetProperties(Flag);
        }
        public MethodInfo Setter(string name)
        {
            return RealType.GetProperty(name, Flag).SetMethod;
        }
        public bool CheckAvalibale(FieldInfo info)
        {
            //readonly const 内部私有类 不做操作
            
            if (info.IsInitOnly || (info.IsLiteral && info.IsStatic) || RealType.GetTypeInfo().IsNestedPrivate)
            {
                return false;
            }
            return true;
        }
    }
    public class Reflector<T>:Reflector
    {
        public Reflector():base(typeof(T))
        {

        }
    }
}
