using Natasha;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ArrayTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DTestArray();
            TestDefault();            //---值类型默认字段测试
            TestJudge();              //---判断测试 @1
            ForeachStringArray();     //---string数组以及遍历测试
            ForeachIntArray();        //---int数组以及遍历测试
            ForeachClassArray();      //---类数组、遍历、深度拷贝测试
            Console.ReadKey();
        }
        public static void TestDefault()
        {
            Action action = (Action)(EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod methodInfoHelper = typeof(Console);
                EVar stringHandler = "16";
                EVar intHandler = 10;
                EVar doubleHandler = 0.00;
                EJudge.If(EDefault.IsDefault(doubleHandler.TypeHandler, () => { doubleHandler.Load(); }))(() =>
                {
                    EMethod.Load(typeof(Console)).ExecuteMethod<string>("WriteLine", "doubleHandler是默认值");

                }).Else(() =>
                {
                    doubleHandler.This();
                    methodInfoHelper.ExecuteMethod<double>("WriteLine");
                });
            }).Compile());
            action();
        }
        public static void TestJudge()
        {
            Action action = (Action)(EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod methodInfoHelper = typeof(Console);
                EVar Int_1 = 2;
                EVar Int_2 = 2;

                EArray objectArray = EArray.CreateArraySpecifiedLength<object>(2);
                objectArray.StoreArray(0, Int_1.InStackAndPacket);
                objectArray.StoreArray(1, Int_2.InStackAndPacket);

                EJudge.
                If(Int_1 > 2)(() =>
                {
                    EVar str = "{0}>{1}";
                    methodInfoHelper.ExecuteMethod<string, object[]>("WriteLine", str, objectArray);
                })
                .ElseIf(Int_1 == Int_2)(() =>
                {
                    EVar str = "{0}={1}";
                    methodInfoHelper.ExecuteMethod<string, object[]>("WriteLine", str, objectArray);
                })
                .Else(() =>
                {
                    EVar str = "{0}<{1}";
                    methodInfoHelper.ExecuteMethod<string, object[]>("WriteLine", str, objectArray);
                });
            }).Compile());
            action();
        }
       


        public static void ForeachStringArray()
        {
            string[] testArray = new string[] { "1", "2", "C", "D" };
            Delegate ShowDelegate = EHandler.CreateMethod<ENull>((il) =>
            {
                EArray Model = testArray;
                ELoop.For(Model, (loadCurrentElement) =>
                {
                    loadCurrentElement();
                    Model.il.REmit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
                });
            }).Compile();
            ((Action)ShowDelegate)();
        }
        public static void ForeachIntArray()
        {
            int[] testArray = new int[] { 1, 2, 3, 4, 5 };
            Delegate ShowDelegate = EHandler.CreateMethod<ENull>((il) =>
            {
                EArray Model = testArray;
                ELoop.For(Model, (loadCurrentElement) =>
                {
                    loadCurrentElement();
                    Model.il.REmit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }));
                });
            }).Compile();
            ((Action)ShowDelegate)();
        }
        public static void ForeachClassArray()
        {
            TestClass[] testArray = new TestClass[5];
            for (int i = 0; i < testArray.Length; i += 1)
            {
                TestClass t = new TestClass();
                t.Name = "T" + i;
                testArray[i] = t;
            }
            testArray[0].FieldNext = new TestStruct() { Name = "1", Age = 10 };
            Delegate ShowDelegate = EHandler.CreateMethod<TestClass[]>((il) =>
            {
                EArray model = testArray;
                ELoop.For(model, (loadCurrentElement) =>
                {
                    EModel modelHandler = EModel.CreateModelFromAction<TestClass>(loadCurrentElement);
                    modelHandler.LField("FieldNext").LFieldValue("Age");
                    modelHandler.il.REmit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }));
                });
                model.Load();
            }).Compile();

            if (ShowDelegate != null)
            {
                TestClass[] result = ((Func<TestClass[]>)ShowDelegate)();
                for (int i = 0; i < result.Length; i += 1)
                {
                    result[i].Name = "T" + (i + 5);
                }
                Console.WriteLine(result[0].FieldNext.Age);
                Console.WriteLine("旧对象：");
                for (int i = 0; i < testArray.Length; i += 1)
                {
                    Console.WriteLine(testArray[i].Name);
                }
                Console.WriteLine("深度复制新对象：");
                for (int i = 0; i < result.Length; i += 1)
                {
                    Console.WriteLine(result[i].Name);
                }
            }
            else
            {
                Console.WriteLine("??");
            }
        }

        public static void DTestArray()
        {
            string[] strArray = new string[5];
            int[] intArray = new int[5];
            StructField[] structArray = new StructField[5];
            ClassField[] classArray = new ClassField[5];
            DocumentEnum[] enumArray = new DocumentEnum[5];
            for (int i = 0; i < strArray.Length; i += 1)
            {
                strArray[i] = i.ToString();
                intArray[i] = i;
                enumArray[i] = DocumentEnum.ID;
                structArray[i] = new StructField() { PublicAge = i };
                classArray[i] = new ClassField() { PublicAge = i };
            }
            //动态创建Action委托
            Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                //从运行时获取数组并入栈到IL层临时变量
                EArray stringArrayModel = strArray;
                ELoop.For(stringArrayModel, (currentElement) =>
                {
                    method.ExecuteMethod<string>("WriteLine", currentElement);
                });

                EArray intArrayModel = intArray;
                ELoop.For(3, 5, 1, intArrayModel, (currentElement) =>
                {
                    method.ExecuteMethod<int>("WriteLine", currentElement);
                });

                EArray arrayModel = EArray.CreateArraySpecifiedLength<int>(10);
                arrayModel.StoreArray(5, 6);
                arrayModel.StoreArray(6, 6);
                arrayModel.StoreArray(7, 6);
                ELoop.For(0, 10, 1, arrayModel, (currentElement) =>
                {
                    method.ExecuteMethod<int>("WriteLine", currentElement);
                });
                //从运行时获取数组并入栈到IL层临时变量
                EArray structArrayModel = EArray.CreateArrayFromRuntimeArray(structArray);
                ELoop.For(structArrayModel, (currentElement) =>
                {
                    EModel model = EModel.CreateModelFromAction(currentElement, typeof(StructField));
                    model.LFieldValue("PublicAge");
                    method.ExecuteMethod<int>("WriteLine");
                });
                EArray classArrayModel = EArray.CreateArrayFromRuntimeArray(classArray);
                ELoop.For(classArrayModel, (currentElement) =>
                {
                    EModel model = EModel.CreateModelFromAction(currentElement, typeof(ClassField));
                    model.LFieldValue("PublicAge");
                    method.ExecuteMethod<int>("WriteLine");
                });
                EArray enumArrayModel = EArray.CreateArrayFromRuntimeArray(enumArray);
                ELoop.For(enumArrayModel, (currentElement) =>
                {
                    EModel model = EModel.CreateModelFromAction(currentElement, typeof(DocumentEnum));
                    model.Load();
                    EPacket.Packet(typeof(DocumentEnum));
                    method.ExecuteMethod<object>("WriteLine");
                });
            }).Compile();

            ((Action)newMethod)();

        }
    }
}
