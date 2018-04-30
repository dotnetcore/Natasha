using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatashaUT.Model
{
    public class ClassWithNullableModel
    {
        public ClassWithNullableModel()
        {
            PrivateProperty = 1;
        }
        public ulong? ValueField;
        public ulong? ValueProperty { get; set; }

        public static ulong? StaticValueField;

        public static ulong? StaticValueProperty { get; set; }

        private ulong? PrivateProperty { get; set; }

    }
}
