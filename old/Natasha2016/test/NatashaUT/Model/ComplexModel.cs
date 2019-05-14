using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatashaUT.Model
{
    public class NestingClassModel
    {
        public FieldClass FieldModel;
        public PropertyClass PropertyModel;
        public MethodClass MethodModel;
        public static NestingClassModel Model;
    }

    public struct NestingStructModel
    {
        public FieldStruct FieldModel;
        public PropertyStruct PropertyModel;
        public MethodStruct MethodModel;
        public static NestingStructModel Model;
    }

    public class ComplexClassModel
    {
        public FieldStruct FieldModel;
        public PropertyStruct PropertyModel;
        public MethodStruct MethodModel;
        public static ComplexClassModel Model;
    }
    public struct ComplexStructModel
    {
        public FieldClass FieldModel;
        public PropertyClass PropertyModel;
        public MethodClass MethodModel;
        public static ComplexStructModel Model;
    }
}
