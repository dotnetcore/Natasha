using Natasha;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NatashaUT.Complex
{
    [Trait("赋值/加载", "类")]
    public class CLSTest
    {
        [Fact(DisplayName ="字段")]
        public void TestField()
        {
            FieldClass model = new FieldClass();
            model.ValueField = ulong.MaxValue;
            model.RefField = "Test";
            FieldClass.StaticRefField = "Static";
            FieldClass.StaticValueField = ulong.MinValue;
            Delegate test = EHandler.CreateMethod<FieldClass>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Set("ValueField", ulong.MaxValue);
                modelHandler.Set("StaticValueField", ulong.MinValue);
                modelHandler.Set("RefField", modelHandler.DLoadValue("RefField").Operator + modelHandler.DLoadValue("StaticRefField").DelayAction);
                modelHandler.Load();
            }).Compile();
            Func<FieldClass> action = (Func<FieldClass>)test;
            FieldClass result = action();
            Assert.Equal(ulong.MaxValue, result.ValueField);
            Assert.Equal(ulong.MinValue, FieldClass.StaticValueField);
            Assert.Equal("TestStatic", result.RefField);

        }
        [Fact(DisplayName = "属性")]
        public void TestProperty()
        {
            PropertyClass model = new PropertyClass();
            model.ValueProperty = ulong.MaxValue;
            model.RefProperty = "Test";
            PropertyClass.StaticRefProeprty = "Static";
            PropertyClass.StaticValueProperty = ulong.MinValue;
            Delegate test = EHandler.CreateMethod<PropertyClass>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Set("ValueProperty", ulong.MaxValue);
                modelHandler.Set("StaticValueProperty", ulong.MinValue);
                modelHandler.Set("RefProperty", modelHandler.DLoad("RefProperty").Operator + modelHandler.DLoad("StaticRefProeprty").DelayAction);
                modelHandler.Load();
            }).Compile();
            Func<PropertyClass> action = (Func<PropertyClass>)test;
            PropertyClass result = action();
            Assert.Equal(ulong.MaxValue, result.ValueProperty);
            Assert.Equal(ulong.MinValue, PropertyClass.StaticValueProperty);
            Assert.Equal("TestStatic", result.RefProperty);
        }
        [Fact(DisplayName = "方法")]
        public void TestMethod()
        {
            Delegate test = EHandler.CreateMethod<ulong>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<MethodClass>().UseDefaultConstructor();
                EMethod.Load(modelHandler).ExecuteMethod("GetULongMax");
            }).Compile();
            Func<ulong> action = (Func<ulong>)test;
            Assert.Equal(ulong.MaxValue, action());

            test = EHandler.CreateMethod<ulong>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<MethodClass>().UseDefaultConstructor();
                EMethod.Load(modelHandler).ExecuteMethod("GetULongMin");
            }).Compile();
            action = (Func<ulong>)test;
            Assert.Equal(ulong.MinValue, action());

            test = EHandler.CreateMethod<string>((il) =>
            {
                EModel modelHandler = EModel.CreateModel<MethodClass>().UseDefaultConstructor();
                EVar param1 = "Hello";
                EMethod.Load(modelHandler).ExecuteMethod<string,string>("GetString", param1," World");
            }).Compile();
            Func<string> action1 = (Func<string>)test;
            Assert.Equal("Hello World", action1());
        }
        //[Fact(DisplayName = "测试类的委托")]
        //public void TestDelegate()
        //{
        //    Delegate test = EHandler.CreateMethod<ulong>((il) =>
        //    {
        //        EModel modelHandler = EModel.CreateModel<DelegateClass>().UseDefaultConstructor();
        //        modelHandler.Load("GetMax").ExecuteDelegate();
        //    }).Compile();
        //    Func<ulong> action = (Func<ulong>)test;
        //    Assert.Equal(ulong.MaxValue, action());

        //    test = EHandler.CreateMethod<ulong>((il) =>
        //    {
        //        EModel modelHandler = EModel.CreateModel<DelegateClass>().UseDefaultConstructor();
        //        modelHandler.Load("GetMin").ExecuteDelegate();
        //    }).Compile();
        //    action = (Func<ulong>)test;
        //    Assert.Equal(ulong.MinValue, action());
        //}
    }

    [Trait("运算", "类")]
    public class ClOTest
    {
        [Fact(DisplayName = "字段")]
        public void TestField()
        {
            FieldClass model = new FieldClass();
            model.ValueField = 100;
            model.RefField = "Test";
            FieldClass.StaticRefField = "Static";
            FieldClass.StaticValueField = 200;
            Delegate test = EHandler.CreateMethod<FieldClass>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Set("ValueField", modelHandler.DLoad("ValueField").Operator + modelHandler.DLoad("StaticValueField").DelayAction);
                modelHandler.Set("StaticValueField", modelHandler.DLoadValue("ValueField").Operator + modelHandler.DLoadValue("StaticValueField").DelayAction);
                modelHandler.Set("RefField", modelHandler.DLoadValue("RefField").Operator + modelHandler.DLoadValue("StaticRefField").DelayAction);
                modelHandler.Load();
            }).Compile();
            Func<FieldClass> action = (Func<FieldClass>)test;
            FieldClass result = action();
            Assert.Equal((ulong)300, result.ValueField);
            Assert.Equal((ulong)500, FieldClass.StaticValueField);
            Assert.Equal("TestStatic", result.RefField);

        }
        [Fact(DisplayName = "属性")]
        public void TestProperty()
        {
            PropertyClass model = new PropertyClass();
            model.ValueProperty = 100;
            model.RefProperty = "Test";
            PropertyClass.StaticRefProeprty = "Static";
            PropertyClass.StaticValueProperty = 200;
            Delegate test = EHandler.CreateMethod<PropertyClass>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Set("ValueProperty", modelHandler.DLoad("ValueProperty").Operator + modelHandler.DLoad("StaticValueProperty").DelayAction);
                modelHandler.Set("StaticValueProperty", modelHandler.DLoad("ValueProperty").Operator + modelHandler.DLoad("StaticValueProperty").DelayAction);
                modelHandler.Set("RefProperty", modelHandler.DLoad("RefProperty").Operator + modelHandler.DLoad("StaticRefProeprty").DelayAction);
                modelHandler.Load();
            }).Compile();
            Func<PropertyClass> action = (Func<PropertyClass>)test;
            PropertyClass result = action();
            Assert.Equal((ulong)300, result.ValueProperty);
            Assert.Equal((ulong)500, PropertyClass.StaticValueProperty);
            Assert.Equal("TestStatic", result.RefProperty);
        }
    }
}
