using Natasha;
using System;

namespace SingleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSingle();             //--普通变量
            TestOperaotr();           //---加减乘除各种运算
            TestNull();               //测试空值
            Console.ReadKey();
        }

        public static void TestSingle()
        {
            //动态创建Action委托
            Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
            {
                //创建没有临时变量的普通入栈变量(没有临时变量，所以自加操作没有意义)
                EVar intHandler = 1;
                //创建函数操作句柄
                EMethod method = typeof(Console);
                //输出intHandler的时候，让变量做加法运算。
                method.ExecuteMethod<int>("WriteLine", intHandler + 665);
                //结果:666；

            }).Compile();

            ((Action)newMethod)();

            //动态创建Action<int,string>委托
            Delegate newMethod1 = EHandler.CreateMethod<ENull>((il) =>
            {
                //创建有临时变量的普通入栈变量(自加操作可以被自身储存) 也就是说可以使用store存储函数
                //int i = 664;
                EVar intHandler = EVar.CreateVarFromObject(664);
                //i++;
                intHandler++;
                //i=i+1;
                intHandler.Store(intHandler + 1);
                //创建函数操作句柄
                EMethod method = typeof(Console);
                //输出intHandler
                method.ExecuteMethod<int>("WriteLine", intHandler);
                //结果:666
            }).Compile();
            ((Action)newMethod1)();

            ////动态创建Action委托
            //Delegate newMethod0 = EHandler.CreateMethod<ENull>((il) => { }).Compile();

            ////动态创建Action<string,int>委托
            //Delegate newMethod1 = EHandler.CreateMethod<string,int,ENull>((il) => { }).Compile();

            ////动态创建Func<string>委托
            //Delegate newMethod2 = EHandler.CreateMethod<string>((il) => { }).Compile();

            ////动态创建Func<string,TestClass>委托
            //Delegate newMethod3 = EHandler.CreateMethod<string, TestClass>((il) => { }).Compile();
        }

        public static void TestOperaotr()
        {

            Delegate showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                EVar emit_A = EVar.CreateWithoutTempVar(12);
                EVar emit_B = EVar.CreateWithoutTempVar(13);
                method.ExecuteMethod<int>("WriteLine", emit_A + emit_B);

            }).Compile();

            ((Action)showResult)();

            showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                EVar emit_A = EVar.CreateWithoutTempVar(12);
                EVar emit_B = EVar.CreateWithoutTempVar(13);
                method.ExecuteMethod<int>("WriteLine", emit_A - emit_B);

            }).Compile();

            ((Action)showResult)();

            showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                EVar emit_A = EVar.CreateWithoutTempVar(12);
                EVar emit_B = EVar.CreateWithoutTempVar(4);
                method.ExecuteMethod<int>("WriteLine", emit_A * emit_B);

            }).Compile();

            showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                EVar emit_A = EVar.CreateWithoutTempVar(12.04);
                EVar emit_B = EVar.CreateWithoutTempVar(4.00);
                method.ExecuteMethod<double>("WriteLine", emit_A / emit_B);

            }).Compile();

            ((Action)showResult)();


            showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                EVar emit_A = EVar.CreateWithoutTempVar(12);
                EVar emit_B = EVar.CreateWithoutTempVar(5);
                method.ExecuteMethod<int>("WriteLine", emit_A % emit_B);

            }).Compile();

            ((Action)showResult)();

            showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                EVar emit_A = EVar.CreateWithoutTempVar(8);
                EVar emit_B = EVar.CreateWithoutTempVar(4);
                method.ExecuteMethod<int>("WriteLine", emit_A >> 1);

            }).Compile();

            ((Action)showResult)();

            showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                EVar emit_A = EVar.CreateWithoutTempVar(8);
                EVar emit_B = EVar.CreateWithoutTempVar(4);
                method.ExecuteMethod<int>("WriteLine", emit_A << 1);

            }).Compile();

            ((Action)showResult)();

            showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                EVar emit_A = EVar.CreateWithoutTempVar(8);
                EVar emit_B = EVar.CreateWithoutTempVar(4);
                method.ExecuteMethod<int>("WriteLine", emit_A | emit_B);

            }).Compile();

            ((Action)showResult)();

            showResult = EHandler.CreateMethod<ENull>((il) =>
            {
                EMethod method = typeof(Console);
                EVar emit_A = EVar.CreateWithoutTempVar(8);
                EVar emit_B = EVar.CreateWithoutTempVar(4);
                method.ExecuteMethod<int>("WriteLine", emit_A & emit_B);

            }).Compile();

            ((Action)showResult)();
        }

        public static void TestNull()
        {
            //动态创建Action委托
            Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
            {
                //创建没有临时变量的普通入栈变量(没有临时变量，所以自加操作没有意义)
                EVar intHandler = EVar.CreateVarFromObject("2");
                //创建函数操作句柄
                EMethod method = typeof(Console);
                //输出intHandler
                method.ExecuteMethod<string>("WriteLine", intHandler);
                //填空值string=null
                intHandler.Store(ENull.Value);
                method.ExecuteMethod<string>("WriteLine", intHandler);

            }).Compile();

            ((Action)newMethod)();
        }
    }
}
