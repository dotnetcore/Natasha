using Natasha;
using Natasha.Utils;
using System;
using System.Collections.Generic;

namespace ComplexTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass();              //---类的测试
            TestClassOperator();      //---类的加减乘除各种运算
            TestClassAndStruct();     //---类和结构体的测试
            StaticClass();            //---静态变量测试
            TestNesting();            //---类嵌套结构体和类的测试
            TestStructIsDefault();    //---结构体是否是不是空的测试
            TestEReflector();         //---EReflector测试
            TestClassClone();         //---类深度拷贝测试
            TestStructClone();        //---结构体深度拷贝测试
            TestComplexClone();       //---复杂结构深度拷贝测试
            TestListClone();          //---List结构深度复制
            //TestDictionaryClone();  //---Dictionary结构深度复制   (X) 测试不通过，因为内部私有类以及比较器的构造函数需要另外提供
            Console.ReadKey();
        }

        public static void TestClass()
        {
            Delegate ShowDelegate = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod methodInfoHelper = typeof(Console);
                EModel model = EModel.CreateModel<TestClass>().UseDefaultConstructor();
                model.Set("Name", "Name");
                model.Load("Name");
                methodInfoHelper.ExecuteMethod<string>("WriteLine");

                model.Set("Age", 10);
                methodInfoHelper.ExecuteMethod<int>("WriteLine", model.Load("Age"));
                methodInfoHelper.ExecuteMethod<int>("WriteLine", model.LoadStruct("FieldNext").Load("Age"));
            }).Compile();
            ((Action)ShowDelegate)();
        }
        public static void TestClassOperator()
        {
            Delegate showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod methodInfoHelper = typeof(Console);
                EModel model = EModel.CreateModel<TestClass>().UseDefaultConstructor();
                model.Set("Age", 10);

                EModel model2 = EModel.CreateModel<TestClass>().UseDefaultConstructor();
                model2.Set("Age", 11);
                methodInfoHelper.ExecuteMethod<int>("WriteLine", model.Load("Age") + model2.Load("Age"));
                methodInfoHelper.ExecuteMethod<int>("WriteLine", model.Load("Age") - model2.Load("Age"));
                methodInfoHelper.ExecuteMethod<int>("WriteLine", model.Load("Age") * model2.Load("Age"));
                methodInfoHelper.ExecuteMethod<int>("WriteLine", model.Load("Age") / model2.Load("Age"));
                methodInfoHelper.ExecuteMethod<int>("WriteLine", model.Load("Age") + 1000);
            }).Compile();

            ((Action)showResult)();
        }
        public static void TestClassAndStruct()
        {
            //动态创建Action委托
            Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
            {

                EModel model = null;
                //测试类的字段
                //model = EModel.CreateModel<ClassField>().UseDefaultConstructor();
                //测试类的属性
                //model = EModel.CreateModel<ClassProperty>().UseDefaultConstructor();
                //测试结构体的字段
                model = EModel.CreateModel<StructField>();
                //测试结构体的属性
                //model = EModel.CreateModel<StructProperty>();
                model.Set("PublicName", "This is Public-Name");
                model.Set("PrivateName", "This is Private-Name");
                model.Set("PublicAge", 666);
                model.Set("PrivateAge", 666);

                EMethod method = typeof(Console);
                method.ExecuteMethod<string>("WriteLine", model.DLoad("PrivateName").DelayAction);
                method.ExecuteMethod<string>("WriteLine", model.Load("PublicName"));
                method.ExecuteMethod<int>("WriteLine", model.Load("PublicAge"));
                method.ExecuteMethod<int>("WriteLine", model.Load("PrivateAge"));

            }).Compile();

            ((Action)newMethod)();
        }
        public static void StaticClass()
        {
            Delegate ShowDelegate = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod methodInfoHelper = typeof(Console);
                EModel test = EModel.CreateModel<TestClass>().UseDefaultConstructor();

                test.SField("NormalField", 10);
                methodInfoHelper.ExecuteMethod<int>("WriteLine", test.Load("NormalField"));

                test.SField("StaticField", 10);
                methodInfoHelper.ExecuteMethod<int>("WriteLine", test.Load("StaticField"));
                test.SField("Ref_StaticField", "10");
                methodInfoHelper.ExecuteMethod<string>("WriteLine", test.Load("Ref_StaticField"));

                test.SProperty("NormalProperty", 10);
                methodInfoHelper.ExecuteMethod<int>("WriteLine", test.Load("NormalProperty"));
                test.SProperty("Ref_NormalProperty", "10");
                methodInfoHelper.ExecuteMethod<string>("WriteLine", test.Load("Ref_NormalProperty"));

                test.SProperty("StaticProperty", 10);
                methodInfoHelper.ExecuteMethod<int>("WriteLine", test.Load("StaticProperty"));
                test.SProperty("Ref_StaticProperty", "10");
                methodInfoHelper.ExecuteMethod<string>("WriteLine", test.Load("Ref_StaticProperty"));
            }).Compile();
            ((Action)ShowDelegate)();
        }
        public static void TestNesting()
        {
            Delegate ShowDelegate = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod methodInfoHelper = typeof(Console);

                EModel structModel = EModel.CreateModel<TestStruct>().UseDefaultConstructor();
                structModel.SProperty("Name", "Name1");
                structModel.SField("Age", 101);


                EModel nesting_classModel = EModel.CreateModel<TestClass>().UseDefaultConstructor();
                nesting_classModel.SProperty("Name", "Name2");
                nesting_classModel.SField("Age", 102);

                EModel classModel = EModel.CreateModel<TestClass>().UseDefaultConstructor();
                classModel.SProperty("Name", "Name");
                classModel.SField("Age", 10);
                classModel.SField("FieldNext", structModel);
                classModel.SProperty("PropertyNext", nesting_classModel);

                classModel.LFieldStructr("FieldNext").LField("Age");
                methodInfoHelper.ExecuteMethod<int>("WriteLine");

                classModel.LFieldStructr("FieldNext").LProperty("Name");
                methodInfoHelper.ExecuteMethod<string>("WriteLine");

                classModel.LProperty("PropertyNext").LField("Age");
                methodInfoHelper.ExecuteMethod<int>("WriteLine");

                classModel.LProperty("PropertyNext").LProperty("Name");
                methodInfoHelper.ExecuteMethod<string>("WriteLine");

            }).Compile();
            ((Action)ShowDelegate)();
        }

        public static void TestStructIsDefault()
        {
            EStruckCheck.Create(typeof(TestStruct));
            TestStruct model = new TestStruct();
            model.Age = 1;
            model.Name = "1";
            Console.WriteLine(EStruckCheck.IsDefaultStruct(model));
            TestClass t = new TestClass();
            Console.WriteLine(EStruckCheck.IsDefaultStruct(t.FieldNext));
        }
        public static void TestEReflector()
        {
            EReflector.Create(typeof(TestStruct));
            TestStruct t2 = new TestStruct();
            t2.Name = "值引用类型不吃Set这套，因为它不是ref引用";
            EReflector.SetMethodDict[typeof(TestStruct)]["Name"](t2, "小明1");
            EReflector.SetMethodDict[typeof(TestStruct)]["Age"](t2, 11);
            EReflector.SetMethodDict[typeof(TestStruct)]["Age1"](t2, 22);
            EReflector.SetMethodDict[typeof(TestStruct)]["Name1"](t2, "小明2");
            Console.WriteLine(EReflector.GetMethodDict[typeof(TestStruct)]["Name"](t2));
            Console.WriteLine(EReflector.GetMethodDict[typeof(TestStruct)]["Age"](t2));
            Console.WriteLine(EReflector.GetMethodDict[typeof(TestStruct)]["Name1"](t2));
            Console.WriteLine(EReflector.GetMethodDict[typeof(TestStruct)]["Age1"](t2));
            EReflector.Create(typeof(TestClass));
            TestClass t3 = new TestClass();
            t3.Age = 1;
            t3.FieldNext = new TestStruct() { Age = 1 };
            TestStruct t = (TestStruct)(EReflector.GetMethodDict[typeof(TestClass)]["FieldNext"](t3));
            Console.WriteLine(t.Age);
        }

        public static void TestClassClone()
        {
            SingleModel t = new SingleModel();
            t.TestStructArray = new TestStruct[5];
            t.TestStructArray[3] = new TestStruct { Age = 666 };
            t.TestClassArray = new TestClass[15];
            t.TestClassArray[10] = new TestClass() { Name = "xxxx" };
            t.TestEnumArray = new TestEnum[16];
            t.TestEnumArray[15] = TestEnum.Address;
            t.Name = "小明";
            t.Age = 10;
            t.Set();
            Delegate ShowDelegate = EHandler.CreateMethod<SingleModel>((il) =>
            {
                EModel model = EModel.CreateModelFromObject(t);
                model.Load();
            }).Compile();
            SingleModel t2 = ((Func<SingleModel>)ShowDelegate)();
            Console.WriteLine(t2.Name);
            Console.WriteLine(t2.Age);
            t2.Show();
            Console.WriteLine(t2.TestStructArray[3].Age);
            Console.WriteLine(t2.TestClassArray[10].Name);
            Console.WriteLine(t2.TestEnumArray[15]);
        }
        public static void TestStructClone()
        {
            EReflector.Create(typeof(SingleModel));
            TestStruct t = new TestStruct();
            t.TEnum = TestEnum.Address;
            t.Set();
            t.Name = "小明";
            t.Name1 = "小明1";
            t.Age = 10;
            t.Age1 = 101;

            Delegate ShowDelegate = EHandler.CreateMethod<TestStruct>((il) =>
            {
                EMethod methodInfoHelper = typeof(Console);
                EModel model = EModel.CreateModelFromObject(t);
                model.SField("PrivateFAge", 10);
                model.LField("PrivateFAge");
                methodInfoHelper.ExecuteMethod<int>("WriteLine");
                model.Load();

            }).Compile();
            TestStruct t2 = ((Func<TestStruct>)ShowDelegate)();
            Console.WriteLine(t2.Name);
            Console.WriteLine(t2.Age);
            Console.WriteLine(t2.Name1);
            Console.WriteLine(t2.Age1);
            t2.Show();
            Console.WriteLine(t.TEnum);
            Console.WriteLine(t2.TEnum);


        }
        public static void TestComplexClone()
        {
            TestClass testModel = new TestClass();
            testModel.Name = "X";
            testModel.Age = 10;

            TestStruct nestingStruct = new TestStruct();
            nestingStruct.Age = 101;
            nestingStruct.Name = "CX";

            TestInterfaceModel interfaceModel = new TestInterfaceModel();
            interfaceModel.Shut = true;

            testModel.PInterface = interfaceModel;
            testModel.FieldNext = nestingStruct;
            testModel.PropertyNext1 = nestingStruct;

            Delegate ShowDelegate = EHandler.CreateMethod<TestClass>((il) =>
            {
                EMethod method = typeof(Console);
                EModel classModel = EModel.CreateModelFromObject(testModel);
                classModel.ALoad("PrivatePName").GetAttributeModel("Attribute1").Load("Name");
                method.ExecuteMethod<string>("WriteLine");
                classModel.Load();
            }).Compile();
            TestClass Result = ((Func<TestClass>)ShowDelegate)();
            Console.WriteLine(Result.Name);
            Console.WriteLine(Result.FieldNext.Name);
            Console.WriteLine(Result.FieldNext.Age);
            Console.WriteLine(Result.PropertyNext1.Name);
            Console.WriteLine(Result.PropertyNext1.Age);
            Console.WriteLine(Result.PInterface.Shut);
        }
        public static void TestListClone()
        {
            List<string> testList = new List<string>();
            testList.Add("One");
            testList.Add("Two");
            testList.Add("Three");
            Delegate ShowDelegate = EHandler.CreateMethod<List<string>>((il) =>
            {
                EModel model = EModel.CreateModelFromObject(testList);
                model.Load();
            }).Compile();
            List<string> t2 = ((Func<List<string>>)ShowDelegate)();
            testList.RemoveAt(0);
            Console.WriteLine(testList[0]);
            Console.WriteLine(t2[0]);
        }
        public static void TestDictionaryClone()
        {
            Dictionary<int, string> testList = new Dictionary<int, string>();
            testList[0] = "One";
            testList[1] = "Two";
            testList[2] = "Three";
            Delegate ShowDelegate = EHandler.CreateMethod<Dictionary<int, string>>((il) =>
            {
                EModel model = EModel.CreateModelFromObject(testList);
                model.Load();
            }).Compile();
            Dictionary<int, string> t2 = ((Func<Dictionary<int, string>>)ShowDelegate)();
            testList.Remove(0);
            Console.WriteLine(testList[0]);
            Console.WriteLine(t2[0]);
        }
    }
}
