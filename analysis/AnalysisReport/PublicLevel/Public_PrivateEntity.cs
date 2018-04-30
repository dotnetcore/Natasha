using System;
using System.Collections.Generic;
using System.Text;

namespace AnalysisReport
{
    public class Public_PrivateEntity
    {

        public Public_PrivateEntity()
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
        private string Field;
        private string Property { get; set; }

        private static string StaticField;
        private static string StaticProperty { get; set; }

        private const string ConstField = "1";

        private readonly string ReadOnlyField;
    }
}
