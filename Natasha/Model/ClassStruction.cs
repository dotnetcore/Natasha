using System;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha
{
    //存储类结构
    public class ClassStruction
    {
        public string Name;
        public Type TypeHandler;
        public bool IsStruct;
        public Dictionary<string, MethodInfo> Methods;
        public Dictionary<string, FieldInfo> Fields;
        public Dictionary<string, PropertyInfo> Properties;
        public Dictionary<string, Dictionary<string, EModel>> AttributeTree;
        public ClassStruction()
        {
            AttributeTree = new Dictionary<string, Dictionary<string, EModel>>();
            Methods = new Dictionary<string, MethodInfo>();
            Fields = new Dictionary<string, FieldInfo>();
            Properties = new Dictionary<string, PropertyInfo>();
        }
    }
}
