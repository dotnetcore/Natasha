using System;
using System.Collections.Generic;
using System.Text;

namespace AnalysisEntity
{
    internal class Internal_ProtectedEntity
    {

        public Internal_ProtectedEntity()
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
        protected string Field;
        protected string Property { get; set; }

        protected static string StaticField;
        protected static string StaticProperty { get; set; }

        protected const string ConstField = "1";

        protected readonly string ReadOnlyField;
    }
}
