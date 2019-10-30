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

        public Func<CallModel, string> OriginGetString;
        public Func<CallModel, DateTime> OriginGetDateTime;
        public Action<CallModel, DateTime> OriginSetDateTime;
        public Action<CallModel, string> OriginSetString;

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

            OriginGetString = item => item.Age;


            NatashaGetDateTime = 
                 FastMethodOperator
                .New
                .Param<CallModel>("obj")
                .MethodBody("return obj.CreateTime;")
                .Return<DateTime>()
                .Complie<Func<CallModel, DateTime>>();

            OriginGetDateTime = item => item.CreateTime;


            NatashaSetString = 
                FastMethodOperator
                .New
                .Param<CallModel>("obj")
                .Param<string>("str")
                .MethodBody("obj.Age=str;")
                .Return()
                .Complie<Action<CallModel, string>>();

           OriginSetString = (item, value) => item.Age = value;

            NatashaSetDateTime = 
                FastMethodOperator
                .New
                .Param<CallModel>("obj")
                .Param<DateTime>("time")
                .MethodBody("obj.CreateTime=time;")
                .Return()
                .Complie<Action<CallModel, DateTime>>();

            OriginSetDateTime = (item, value) => item.CreateTime = value;
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

        [BenchmarkCategory("Write", "String"), Benchmark(Description = "EmitAction")]
        public void EmitFieldSetStringTest()
        {
            EmitSetString(EmitModel, "Hello");
        }

        [BenchmarkCategory("Write", "String"), Benchmark(Description = "Origin")]
        public void OriginFieldSetStringTest()
        {
            OriginModel.Age = "Hello";
        }
        [BenchmarkCategory("Write", "String"), Benchmark(Baseline = true, Description = "OriginAction")]
        public void OriginActionFieldSetStringTest()
        {
            OriginSetString(NatashaModel, "Hello");
        }

        [BenchmarkCategory("Write", "String"), Benchmark(Description = "NatashaAction")]
        public void DynamicFieldSetStringTest()
        {
            NatashaSetString(NatashaModel, "Hello");
        }


        [BenchmarkCategory("Write", "Time"), Benchmark(Description = "EmitAction")]
        public void EmitFieldSetTimeTest()
        {
            EmitSetDateTime(EmitModel, DateTime.Now);
        }

        [BenchmarkCategory("Write", "Time"), Benchmark(Baseline = true, Description = "Origin")]
        public void OriginFieldSetTimeTest()
        {
            OriginModel.CreateTime = DateTime.Now;
        }

        [BenchmarkCategory("Write", "DateTime"), Benchmark(Description = " OriginAction")]
        public void OriginActionFieldSetTimeTest()
        {
            OriginSetDateTime(NatashaProxyModel, DateTime.Now);
        }

        [BenchmarkCategory("Write", "DateTime"), Benchmark(Description = "NatashaAction")]
        public void DynamicFieldSetTimeTest()
        {
            NatashaSetDateTime(NatashaProxyModel, DateTime.Now);
        }

        //#endregion


        //#region 字段读性能
        //[BenchmarkCategory("Read", "String"), Benchmark(Description = "Emit")]
        //public void EmitFieldGetStringTest()
        //{
        //    string result = EmitGetString(EmitModel);
        //}
        //[BenchmarkCategory("Read", "String"), Benchmark(Baseline = true, Description = "Origin")]
        //public void OriginFieldGetStringTest()
        //{
        //    string result = OriginModel.Age;
        //}

        //[BenchmarkCategory("Read", "String"), Benchmark(Description = "NatashaDirectly")]
        //public void DynamicFieldGetStringTest()
        //{
        //    string result = NatashaGetString(NatashaModel);
        //}



        //[BenchmarkCategory("Read", "Time"), Benchmark(Description = "Emit")]
        //public void EmitFieldGetTimeTest()
        //{
        //    DateTime result = EmitGetDateTime(EmitModel);
        //}
        //[BenchmarkCategory("Read", "Time"), Benchmark(Baseline = true, Description = "Origin")]
        //public void OriginFieldGetTimeTest()
        //{
        //    DateTime result = OriginModel.CreateTime;
        //}
        //[BenchmarkCategory("Read", "Time"), Benchmark(Description = "NatashaDirectly")]
        //public void DynamicFieldGetTimeTest()
        //{
        //    DateTime result = NatashaGetDateTime(NatashaModel);
        //}

        #endregion
        //[BenchmarkCategory("Read", "Time"), Benchmark(Baseline = true, Description = "Origin")]
        //public void OriginFieldGetTimeTest()
        //{
        //    DateTime result = OriginModel.CreateTime;
        //}
    }
}
