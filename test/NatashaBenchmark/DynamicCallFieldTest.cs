using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using Natasha;
using Natasha.Operator;
using NatashaBenchmark.Model;
using System;
using System.Reflection.Emit;

namespace NatashaBenchmark
{
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
        public Action<CallModel, DateTime> NatashaSetDateTime;
        public Action<CallModel, string> NatashaSetString;

        public CallModel EmitModel;
        public CallModel OriginModel;
        public CallModel NatashaModel;
        public CallModel NatashaProxyModel;

        //public DynamicOperator<CallModel> NatashaCaller;
        //public DynamicOperator DynamicObjectCaller;
        //public DynamicOperatorBase DynamicStrongCaller;
        //public DynamicBase DynamicEntityCaller;
        public DynamicCallFieldTest()
        {
            Precache();
            Preheating();
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


            NatashaGetString = 
                FastMethodOperator
                .New
                .Param<CallModel>("obj")
                .MethodBody("return obj.Age;")
                .Return<string>()
                .Complie<Func<CallModel, string>>();

            NatashaGetDateTime = 
                 FastMethodOperator
                .New
                .Param<CallModel>("obj")
                .MethodBody("return obj.CreateTime;")
                .Return<DateTime>()
                .Complie<Func<CallModel, DateTime>>();

            NatashaSetString = 
                FastMethodOperator
                .New
                .Param<CallModel>("obj")
                .Param<string>("str")
                .MethodBody("obj.Age=str;")
                .Return()
                .Complie<Action<CallModel, string>>();

            NatashaSetDateTime = 
                FastMethodOperator
                .New
                .Param<CallModel>("obj")
                .Param<DateTime>("time")
                .MethodBody("obj.CreateTime=time;")
                .Return()
                .Complie<Action<CallModel, DateTime>>();


        }
        public void Preheating()
        {
            OriginModel = new CallModel();
            EmitModel = new CallModel();
            NatashaModel = new CallModel();
            NatashaProxyModel = new CallModel();

            ////DynamicObjectCaller = new DynamicOperator(typeof(CallModel));

            ////NatashaCaller = new DynamicOperator<CallModel>();
            ////DynamicStrongCaller = DynamicOperator.GetOperator(typeof(CallModel));
            ////DynamicEntityCaller = EntityOperator.Create(typeof(CallModel));
        }

        #region 字段写性能

        [BenchmarkCategory("Write", "String"), Benchmark(Description = "Emit")]
        public void EmitFieldSetStringTest()
        {
            EmitSetString(EmitModel, "Hello");
        }
        [BenchmarkCategory("Write", "String"), Benchmark(Baseline = true, Description = "Origin")]
        public void OriginFieldSetStringTest()
        {
            OriginModel.Age = "Hello";
        }
        [BenchmarkCategory("Write", "String"), Benchmark(Description = "NatashaProxy")]
        public void DynamicFieldProxySetStringTest()
        {
            NatashaSetString(OriginModel, "Hello");
        }
        //////[BenchmarkCategory("Write", "String"), Benchmark(Description = "NatashaStrongDynamicProxy")]
        ////public void DynamicStrongFieldProxySetStringTest()
        ////{
        ////    DynamicStrongCaller["Age"].StringValue = "Hello";
        ////}
        //////[BenchmarkCategory("Write", "String"), Benchmark(Description = "NatashaObjectDynamicProxy")]
        ////public void DynamicObjectFieldProxySetStringTest()
        ////{
        ////    DynamicObjectCaller["Age"].StringValue = "Hello";
        ////}
        //////[BenchmarkCategory("Write", "String"), Benchmark(Description = "NatashaDirectly")]
        ////public void DynamicFieldSetStringTest()
        ////{
        ////    NatashaSetString(NatashaModel, "Hello");
        ////}

        //////[BenchmarkCategory("Write", "String"), Benchmark(Description = "DynamicEntityCaller")]
        ////public void DynamicEntitySetStringTest()
        ////{
        ////    DynamicEntityCaller.Set("Age", "Hello");
        ////}

        ////[BenchmarkCategory("Write", "Time"), Benchmark(Description = "Emit")]
        ////public void EmitFieldSetTimeTest()
        ////{
        ////    EmitSetDateTime(EmitModel, DateTime.Now);
        ////}

        //////[BenchmarkCategory("Write", "Time"), Benchmark(Baseline = true, Description = "Origin")]
        ////public void OriginFieldSetTimeTest()
        ////{
        ////    OriginModel.CreateTime = DateTime.Now;
        ////}
        //////[BenchmarkCategory("Write", "Time"), Benchmark(Description = "NatashaProxy")]
        ////public void DynamicFieldProxySetTimeTest()
        ////{
        ////    NatashaCaller["CreateTime"].DateTimeValue = DateTime.Now;
        ////}
        //////[BenchmarkCategory("Write", "Time"), Benchmark(Description = "NatashaStrongDynamicProxy")]
        ////public void DynamicStrongFieldProxySetTimeTest()
        ////{
        ////    DynamicStrongCaller["CreateTime"].DateTimeValue = DateTime.Now;
        ////}
        //////[BenchmarkCategory("Write", "Time"), Benchmark(Description = "NatashaObjectDynamicProxy")]
        ////public void DynamicObjectFieldProxySetTimeTest()
        ////{
        ////    DynamicObjectCaller["CreateTime"].DateTimeValue = DateTime.Now;
        ////}
        //////[BenchmarkCategory("Write", "DateTime"), Benchmark(Description = "NatashaDirectly")]
        ////public void DynamicFieldSetTimeTest()
        ////{
        ////    NatashaSetDateTime(NatashaProxyModel, DateTime.Now);
        ////}
        //////[BenchmarkCategory("Write", "DateTime"), Benchmark(Description = "DynamicEntityCaller")]
        ////public void DynamicEntitySetTimeTest()
        ////{
        ////    DynamicEntityCaller.Set("CreateTime", DateTime.Now);
        ////}
        ////#endregion


        ////#region 字段读性能
        ////[BenchmarkCategory("Read", "String"), Benchmark(Description = "Emit")]
        ////public void EmitFieldGetStringTest()
        ////{
        ////    string result = EmitGetString(EmitModel);
        ////}
        ////[BenchmarkCategory("Read", "String"), Benchmark(Baseline = true, Description = "Origin")]
        ////public void OriginFieldGetStringTest()
        ////{
        ////    string result = OriginModel.Age;
        ////}
        //////[BenchmarkCategory("Read", "String"), Benchmark(Description = "NatashaProxy")]
        ////public void DynamicFieldProxyGetStringTest()
        ////{
        ////    string result = NatashaCaller["Age"].StringValue;
        ////}
        //////[BenchmarkCategory("Read", "String"), Benchmark(Description = "NatashaStrongDynamicProxy")]
        ////public void DynamicStrongFieldProxyGetStringTest()
        ////{
        ////    string result = DynamicStrongCaller["Age"].StringValue;
        ////}
        //////[BenchmarkCategory("Read", "String"), Benchmark(Description = "NatashaObjectDynamicProxy")]
        ////public void DynamicObjectFieldProxyGetStringTest()
        ////{
        ////    string result = DynamicObjectCaller["Age"].StringValue;
        ////}
        //////[BenchmarkCategory("Read", "String"), Benchmark(Description = "NatashaDirectly")]
        ////public void DynamicFieldGetStringTest()
        ////{
        ////    string result = NatashaGetString(NatashaModel);
        ////}
        //////[BenchmarkCategory("Read", "String"), Benchmark(Description = "DynamicEntityCaller")]
        ////public void DynamicEntityGetStringTest()
        ////{
        ////    DynamicEntityCaller.Get<string>("Age");
        ////}


        ////[BenchmarkCategory("Read", "Time"), Benchmark(Description = "Emit")]
        ////public void EmitFieldGetTimeTest()
        ////{
        ////    DateTime result = EmitGetDateTime(EmitModel);
        ////}
        ////[BenchmarkCategory("Read", "Time"), Benchmark(Baseline = true, Description = "Origin")]
        ////public void OriginFieldGetTimeTest()
        ////{
        ////    DateTime result = OriginModel.CreateTime;
        ////}
        //////[BenchmarkCategory("Read", "Time"), Benchmark(Description = "NatashaProxy")]
        ////public void DynamicFieldProxyGetTimeTest()
        ////{
        ////    DateTime result = NatashaCaller["CreateTime"].DateTimeValue;
        ////}
        //////[BenchmarkCategory("Read", "Time"), Benchmark(Description = "NatashaStrongProxy")]
        ////public void DynamicStrongFieldProxyGetTimeTest()
        ////{
        ////    DateTime result = DynamicStrongCaller["CreateTime"].DateTimeValue;
        ////}
        //////[BenchmarkCategory("Read", "Time"), Benchmark(Description = "NatashaObjectProxy")]
        ////public void DynamicObjectFieldProxyGetTimeTest()
        ////{
        ////    DateTime result = DynamicObjectCaller["CreateTime"].DateTimeValue;
        ////}
        //////[BenchmarkCategory("Read", "Time"), Benchmark(Description = "NatashaDirectly")]
        ////public void DynamicFieldGetTimeTest()
        ////{
        ////    DateTime result = NatashaGetDateTime(NatashaModel);
        ////}
        //////[BenchmarkCategory("Read", "Time"), Benchmark(Description = "DynamicEntityCaller")]
        ////public void DynamicEntityGetTimeTest()
        ////{
        ////    DynamicEntityCaller.Get<DateTime>("CreateTime");
        ////}

        #endregion
        //[BenchmarkCategory("Read", "Time"), Benchmark(Baseline = true, Description = "Origin")]
        //public void OriginFieldGetTimeTest()
        //{
        //    DateTime result = OriginModel.CreateTime;
        //}
    }
}
