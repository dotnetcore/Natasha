using Natasha;
using NatashaUT.Model;
using System;
using Xunit;

namespace NatashaUT.Complex
{
    [Trait("赋值/加载", "结构体")]
    public class SLSTest
    {
        [Fact(DisplayName = "字段")]
        public void TestField()
        {
            Delegate test = EHandler.CreateMethod<FieldStruct>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<FieldStruct>().UseDefaultConstructor();
                modelHandler.Set("ValueField", ulong.MinValue);
                modelHandler.Set("StaticValueField", ulong.MaxValue);
                modelHandler.Set("RefField", "Hello World");
                modelHandler.Load();
            }).Compile();
            Func<FieldStruct> action = (Func<FieldStruct>)test;
            FieldStruct result = action();
            Assert.Equal(ulong.MinValue, result.ValueField);
            Assert.Equal(ulong.MaxValue, FieldStruct.StaticValueField);
            Assert.Equal("Hello World", result.RefField);
        }
        [Fact(DisplayName = "属性")]
        public void TestProperty()
        {
            Delegate test = EHandler.CreateMethod<PropertyStruct>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<PropertyStruct>().UseDefaultConstructor();
                modelHandler.Set("ValueProperty", ulong.MinValue);
                modelHandler.Set("StaticValueProperty", ulong.MaxValue);
                modelHandler.Set("RefProperty", "Hello World");
                modelHandler.Load();
            }).Compile();
            Func<PropertyStruct> action = (Func<PropertyStruct>)test;
            PropertyStruct result = action();
            Assert.Equal(ulong.MinValue, result.ValueProperty);
            Assert.Equal(ulong.MaxValue, PropertyStruct.StaticValueProperty);
            Assert.Equal("Hello World", result.RefProperty);
        }
        [Fact(DisplayName = "方法")]
        public void TestMethod()
        {
            Delegate test = EHandler.CreateMethod<ulong>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<MethodStruct>().UseDefaultConstructor();
                EMethod.Load(modelHandler).ExecuteMethod("GetULongMax");
            }).Compile();
            Func<ulong> action = (Func<ulong>)test;
            Assert.Equal(ulong.MaxValue, action());

            test = EHandler.CreateMethod<ulong>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<MethodStruct>().UseDefaultConstructor();
                EMethod.Load(modelHandler).ExecuteMethod("GetULongMin");
            }).Compile();
            action = (Func<ulong>)test;
            Assert.Equal(ulong.MinValue, action());

            test = EHandler.CreateMethod<string>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<MethodStruct>().UseDefaultConstructor();
                EVar param1 = "Hello";
                EMethod.Load(modelHandler).ExecuteMethod<string, string>("GetString", param1, " World");
            }).Compile();
            Func<string> action1 = (Func<string>)test;
            Assert.Equal("Hello World", action1());
        }
    }
    [Trait("运算", "结构体")]
    public class SLOTest
    {
        [Fact(DisplayName = "字段")]
        public void TestField()
        {
            FieldStruct model = new FieldStruct();
            model.ValueField = 100;
            model.RefField = "Test";
            FieldStruct.StaticRefField = "Static";
            FieldStruct.StaticValueField = 200;
            Delegate test = EHandler.CreateMethod<FieldStruct>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Set("ValueField", modelHandler.DLoadValue("ValueField").Operator + modelHandler.DLoadValue("StaticValueField").DelayAction);
                modelHandler.Set("StaticValueField", modelHandler.DLoadValue("ValueField").Operator + modelHandler.DLoadValue("StaticValueField").DelayAction);
                modelHandler.Set("RefField", modelHandler.DLoadValue("RefField").Operator + modelHandler.DLoadValue("StaticRefField").DelayAction);
                modelHandler.Load();
            }).Compile();
            Func<FieldStruct> action = (Func<FieldStruct>)test;
            FieldStruct result = action();
            Assert.Equal((ulong)300, result.ValueField);
            Assert.Equal((ulong)500, FieldStruct.StaticValueField);
            Assert.Equal("TestStatic", result.RefField);
        }
        [Fact(DisplayName = "属性")]
        public void TestProperty()
        {
            PropertyStruct model = new PropertyStruct();
            model.ValueProperty = 100;
            model.RefProperty = "Test";
            PropertyStruct.StaticRefProeprty = "Static";
            PropertyStruct.StaticValueProperty = 200;
            Delegate test = EHandler.CreateMethod<PropertyStruct>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Set("ValueProperty", modelHandler.DLoad("ValueProperty").Operator + modelHandler.DLoad("StaticValueProperty").DelayAction);
                modelHandler.Set("StaticValueProperty", modelHandler.DLoad("ValueProperty").Operator + modelHandler.DLoad("StaticValueProperty").DelayAction);
                modelHandler.Set("RefProperty", modelHandler.DLoad("RefProperty").Operator + modelHandler.DLoad("StaticRefProeprty").DelayAction);
                modelHandler.Load();
            }).Compile();
            Func<PropertyStruct> action = (Func<PropertyStruct>)test;
            PropertyStruct.StaticValueProperty = 200;
            PropertyStruct result = action();
            Assert.Equal((ulong)300, result.ValueProperty);
            Assert.Equal((ulong)500, PropertyStruct.StaticValueProperty);
            Assert.Equal("TestStatic", result.RefProperty);
        }
        [Fact(DisplayName = "方法")]
        public void TestMethod()
        {
            Delegate test = EHandler.CreateMethod<ulong>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<MethodStruct>().UseDefaultConstructor();
                EMethod.Load(modelHandler).ExecuteMethod("GetULongMax");
            }).Compile();
            Func<ulong> action = (Func<ulong>)test;
            Assert.Equal(ulong.MaxValue, action());

            test = EHandler.CreateMethod<ulong>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<MethodStruct>().UseDefaultConstructor();
                EMethod.Load(modelHandler).ExecuteMethod("GetULongMin");
            }).Compile();
            action = (Func<ulong>)test;
            Assert.Equal(ulong.MinValue, action());

            test = EHandler.CreateMethod<string>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<MethodStruct>().UseDefaultConstructor();
                EVar param1 = "Hello";
                EMethod.Load(modelHandler).ExecuteMethod<string, string>("GetString", param1, " World");
            }).Compile();
            Func<string> action1 = (Func<string>)test;
            Assert.Equal("Hello World", action1());
        }
    }
}
