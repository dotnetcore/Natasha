using Natasha;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ClassBuilderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCreateModel(); //测试自定义类
            Console.ReadKey();
        }

        public static void TestCreateModel()
        {
            ClassBuilder builder = ClassBuilder.CreateModel("Hello");
            builder.CreateDefaultConstructor();
            builder.CreateField<string>("Age", FieldAttributes.Public);
            builder.CreateProperty<string>("Name");
            builder.CreateMethod<ENull>("Show", MethodAttributes.Public, (classModel) =>
            {
                classModel.SField("Age", "This is Age.");
                classModel.SProperty("Name", "This is name.");
                classModel.LPropertyValue("Name");
                classModel.il.REmit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
                classModel.il.REmit(OpCodes.Ret);
            });
            builder.EndBuilder();
            Delegate ShowDelegate = EHandler.CreateMethod<ENull>((il) =>
            {
                EModel Model = EModel.CreateDynamicClass("Hello").UseDefaultConstructor();
                EMethod.Load(Model).ExecuteMethod("Show");
                Model.LFieldValue("Age");
                Model.il.REmit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            }).Compile();
            ((Action)ShowDelegate)();
        }
    }
}
