using System;
using System.Collections.Generic;
using System.Text;

namespace AnalysisEntity
{
    internal class Internal_PublicEntity
    {

        public Internal_PublicEntity()
        {
            Field = "1";
            Property = "1";
            StaticField = "1";
            StaticProperty = "1";
            ReadOnlyField = "1";
        }

        public string GetFiled()
        {
            return Field;
        }
        public string Field;
        public string Property { get; set; }

        public static string StaticField;
        public static string StaticProperty { get; set; }

        public const string ConstField = "1";

        public readonly string ReadOnlyField;
    }
}
