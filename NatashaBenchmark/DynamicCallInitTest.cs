using BenchmarkDotNet.Attributes;
using Natasha;
using NatashaBenchmark.Model;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace NatashaBenchmark
{
    [MemoryDiagnoser, CoreJob, MarkdownExporter,RPlotExporter]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class DynamicCallInitTest
    {
        public Func<CallModel> EmitFunc;
        public Func<CallModel> NatashaFunc;
        public CallModel EmitModel;
        public CallModel OriginModel;
        public CallModel NatashaModel;

        public DynamicOperator<CallModel> NatasshaCaller;
        public DynamicCallInitTest()
        {
            Precache();
            Preheating();
        }
        public void Precache()
        {
            DynamicMethod method = new DynamicMethod("Db" + Guid.NewGuid().ToString(), typeof(CallModel), new Type[0]);
            ILGenerator il = method.GetILGenerator();
            ConstructorInfo ctor = typeof(CallModel).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            EmitFunc = (Func<CallModel>)(method.CreateDelegate(typeof(Func<CallModel>)));
            NatashaFunc = CtorBuilder.NewDelegate<CallModel>();
        }
        public void Preheating()
        {
            OriginModel = new CallModel();
            EmitModel = EmitFunc();
            NatashaModel = NatashaFunc();
            NatasshaCaller = NatashaModel;
        }


        #region 初始化
        [Benchmark(Description ="EmitInitor")]
        public void EmitInitest()
        {
            CallModel model = EmitFunc();
        }
        [Benchmark(Baseline = true, Description = "OriginInitor")]
        public void OriginInitTest()
        {
            CallModel model = new CallModel();
        }
        [Benchmark(Description = "NatashaInitor")]
        public void DynamicInitTest()
        {
            CallModel model = NatashaFunc();
        }
        #endregion


      
    }
}
