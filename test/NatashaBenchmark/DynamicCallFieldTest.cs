using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using Natasha.CSharp;
using NatashaBenchmark.Model;
using System;
using System.Reflection.Emit;

namespace NatashaBenchmark
{

    public delegate void ValueDelegate(CallModel model, in DateTime value);

    [MemoryDiagnoser, MarkdownExporter, RPlotExporter]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    [CategoriesColumn]
    public class DynamicCallFieldTest
    {
        public Func<CallModel, string> EmitGetString;
        public Func<CallModel, DateTime> EmitGetDateTime;
        public Action<CallModel, DateTime> EmitSetDateTime;
        public Action<CallModel, string> EmitSetString;


        public Func<CallModel, string> NatashaGetString;
        public Func<CallModel, DateTime> NatashaGetDateTime;
        public ValueDelegate NatashaSetDateTime;

        public Action<CallModel, string> NatashaSetString;

        public Func<CallModel, string> OriginGetString;
        public Func<CallModel, DateTime> OriginGetDateTime;
        public ValueDelegate OriginSetDateTime;
        public Action<CallModel, string> OriginSetString;

        public CallModel EmitModel;
        public CallModel OriginModel;
        public CallModel NatashaModel;
        public CallModel OriginProxyModel;


        public DynamicCallFieldTest()
        {
            NatashaInitializer.Preheating();
            Preheating();
            Precache();
        }
        public void Precache()
        {
            Type type = typeof(CallModel);
            DynamicMethod method = new DynamicMethod("GetString", typeof(string), new Type[] { type });
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, type.GetField("Age"));
            il.Emit(OpCodes.Ret);
            EmitGetString = (Func<CallModel, string>)(method.CreateDelegate(typeof(Func<CallModel, string>)));


            method = new DynamicMethod("GetDateTime", typeof(DateTime), new Type[] { type });
            il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, type.GetField("CreateTime"));
            il.Emit(OpCodes.Ret);
            EmitGetDateTime = (Func<CallModel, DateTime>)(method.CreateDelegate(typeof(Func<CallModel, DateTime>)));


            method = new DynamicMethod("SetDateString", null, new Type[] { type, typeof(string) });
            il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, type.GetField("Age"));
            il.Emit(OpCodes.Ret);
            EmitSetString = (Action<CallModel, string>)(method.CreateDelegate(typeof(Action<CallModel, string>)));


            method = new DynamicMethod("SetDateTime", null, new Type[] { type, typeof(DateTime) });
            il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, type.GetField("CreateTime"));
            il.Emit(OpCodes.Ret);
            EmitSetDateTime = (Action<CallModel, DateTime>)(method.CreateDelegate(typeof(Action<CallModel, DateTime>)));


            NatashaGetString = NDelegate.DefaultDomain().Func<CallModel, string>("return arg.Age;");
            NatashaGetString(OriginModel);
            OriginGetString = item => item.Age;
           

            NatashaGetDateTime = NDelegate.DefaultDomain().Func<CallModel, DateTime>("return arg.CreateTime;");
            NatashaGetDateTime(OriginModel);
            OriginGetDateTime = item => item.CreateTime;
            

            NatashaSetString = NDelegate.DefaultDomain().Action<CallModel, string>("arg1.Age=arg2;");
            NatashaSetString(OriginModel, OriginModel.Age);
            OriginSetString = (item, value) => item.Age = value;

            //NatashaSetDateTime = DelegateOperator<ValueDelegate>.Delegate("model.CreateTime=value;");
            //NatashaSetDateTime(OriginModel, OriginModel.CreateTime);
            //OriginSetDateTime = OriginDateTime;

        }

        public static void OriginDateTime(CallModel model, in DateTime value)
        {
            model.CreateTime = value;
        }

        public void Preheating()
        {
            OriginModel = new CallModel();
            EmitModel = new CallModel();
            NatashaModel = new CallModel();
            OriginProxyModel = new CallModel();
        }

        #region 字段写性能

        [BenchmarkCategory("Write", "String"), Benchmark(Description = "EmitAction")]
        public void EmitFieldSetStringTest()
        {
            EmitSetString(EmitModel, "Hello");
        }

        [BenchmarkCategory("Write", "String"), Benchmark(Description = "NatashaAction")]
        public void DynamicFieldSetStringTest()
        {
            NatashaSetString(NatashaModel, "Hello");
        }

        [BenchmarkCategory("Write", "String"), Benchmark(Description = "Origin")]
        public void OriginFieldSetStringTest()
        {
            OriginModel.Age = "Hello";
        }
        [BenchmarkCategory("Write", "String"), Benchmark(Baseline = true, Description = "OriginAction")]
        public void OriginActionFieldSetStringTest()
        {
            OriginSetString(OriginProxyModel, "Hello");
        }




        [BenchmarkCategory("Write", "DateTime"), Benchmark(Description = "EmitAction")]
        public void EmitFieldSetTimeTest()
        {
            EmitSetDateTime(EmitModel, DateTime.Now);
        }

        [BenchmarkCategory("Write", "DateTime"), Benchmark(Description = "NatashaAction")]
        public void DynamicFieldSetTimeTest()
        {
            NatashaSetDateTime(NatashaModel, DateTime.Now);
        }

        [BenchmarkCategory("Write", "DateTime"), Benchmark(Description = "Origin")]
        public void OriginFieldSetTimeTest()
        {
            OriginModel.CreateTime = DateTime.Now;
        }

        [BenchmarkCategory("Write", "DateTime"), Benchmark(Baseline = true, Description = " OriginAction")]
        public void OriginActionFieldSetTimeTest()
        {
            OriginSetDateTime(OriginProxyModel, DateTime.Now);
        }



        #endregion


        #region 字段读性能
        [BenchmarkCategory("Read", "String"), Benchmark(Description = "EmitAction")]
        public void EmitFieldGetStringTest()
        {
            string result = EmitGetString(EmitModel);
        }

        [BenchmarkCategory("Read", "String"), Benchmark(Description = "NatashaAction")]
        public void DynamicFieldGetStringTest()
        {
            string result = NatashaGetString(NatashaModel);
        }

        [BenchmarkCategory("Read", "String"), Benchmark(Description = "Origin")]
        public void OriginFieldGetStringTest()
        {
            string result = OriginModel.Age;
        }
        [BenchmarkCategory("Read", "String"), Benchmark(Baseline = true, Description = "OriginAction")]
        public void OriginActionFieldGetStringTest()
        {
            string result = OriginGetString(OriginProxyModel);
        }





        [BenchmarkCategory("Read", "Time"), Benchmark(Description = "EmitAction")]
        public void EmitFieldGetTimeTest()
        {
            DateTime result = EmitGetDateTime(EmitModel);
        }
        [BenchmarkCategory("Read", "Time"), Benchmark(Description = "NatashaAction")]
        public void DynamicFieldGetTimeTest()
        {
            DateTime result = NatashaGetDateTime(NatashaModel);
        }
        [BenchmarkCategory("Read", "Time"), Benchmark(Description = "Origin")]
        public void OriginFieldGetTimeTest()
        {
            DateTime result = OriginModel.CreateTime;
        }
        [BenchmarkCategory("Read", "Time"), Benchmark(Baseline = true, Description = "OriginAction")]
        public void OriginActionFieldGetTimeTest()
        {
            DateTime result = OriginGetDateTime(OriginProxyModel);
        }


        #endregion

    }
}
