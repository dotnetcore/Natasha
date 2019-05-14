using Natasha;
using NatashaUT.Model;
using System;
using Xunit;

namespace NatashaUT.Complex
{
    [Trait("赋值/加载", "单纯嵌套结构")]
    public class NestingTest
    {
        [Fact(DisplayName = "结构体")]
        public void TestStruct()
        {
            NestingStructModel model = new NestingStructModel();
            model.FieldModel = new FieldStruct();
            model.PropertyModel = new PropertyStruct();
            model.MethodModel = new MethodStruct();
            NestingStructModel.Model = model;

            Delegate test = EHandler.CreateMethod<NestingStructModel>((il) =>
            {
                EVar ulongMinHandler = ulong.MinValue;
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Load("FieldModel").Set("RefField", "Hello");
                modelHandler.Load("PropertyModel").Set("RefProperty", "Hello");
                modelHandler.Load("PropertyModel").Set("ValueProperty", ulongMinHandler);
                modelHandler.Load("Model").Load("FieldModel").Set("RefField", "Hello1");
                modelHandler.Load("Model").Load("FieldModel").Set("ValueField", () => { EMethod.Load(modelHandler.DLoad("MethodModel").Operator).ExecuteMethod("GetULongMax"); });
                modelHandler.Load();
            }).Compile();
            Func<NestingStructModel> action = (Func<NestingStructModel>)test;
            NestingStructModel result = action();

            Assert.Equal((ulong)0, result.FieldModel.ValueField);
            Assert.Equal(ulong.MinValue, result.PropertyModel.ValueProperty);
            Assert.Equal("Hello", result.FieldModel.RefField);
            Assert.Equal("Hello", result.PropertyModel.RefProperty);
            Assert.Equal(ulong.MaxValue, NestingStructModel.Model.FieldModel.ValueField);
        }
        [Fact(DisplayName = "类")]
        public void TestClass()
        {
            NestingClassModel model = new NestingClassModel();
            model.FieldModel = new FieldClass();
            model.PropertyModel = new PropertyClass();
            model.MethodModel = new MethodClass();
            NestingClassModel.Model = model;

            Delegate test = EHandler.CreateMethod<NestingClassModel>((il) =>
            {
                EVar ulongMinHandler = ulong.MinValue;
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Load("FieldModel").Set("RefField", "Hello");
                modelHandler.Load("PropertyModel").Set("RefProperty", "Hello");
                modelHandler.Load("PropertyModel").Set("ValueProperty", ulongMinHandler);
                modelHandler.Load("Model").Load("FieldModel").Set("RefField", "Hello1");
                modelHandler.Load("Model").Load("FieldModel").Set("ValueField", () => { EMethod.Load(modelHandler.DLoad("MethodModel").Operator).ExecuteMethod("GetULongMax"); });
                modelHandler.Load();

            }).Compile();
            Func<NestingClassModel> action = (Func<NestingClassModel>)test;
            NestingClassModel result = action();

            Assert.Equal((ulong)0, result.FieldModel.ValueField);
            Assert.Equal(ulong.MinValue, result.PropertyModel.ValueProperty);
            Assert.Equal("Hello", result.FieldModel.RefField);
            Assert.Equal("Hello", result.PropertyModel.RefProperty);
            Assert.Equal(ulong.MaxValue, NestingClassModel.Model.FieldModel.ValueField);
            Assert.Equal("Hello1", NestingClassModel.Model.FieldModel.RefField);
        }
    }
    [Trait("运算", "单纯嵌套结构")]
    public class NestingOperatorTest
    {
        [Fact(DisplayName = "结构体", Skip = "结构体嵌套结构体无法做出是否为默认结构体的判断")]
        public void TestStructOperator()
        {
            NestingStructModel model = new NestingStructModel();
            model.FieldModel = new FieldStruct() { RefField = "Hello", ValueField = ulong.MaxValue - 1 };
            model.PropertyModel = new PropertyStruct() { RefProperty = "World", ValueProperty = ulong.MinValue + 1 };
            model.MethodModel = new MethodStruct();
            NestingStructModel.Model = model;

            Delegate test = EHandler.CreateMethod<NestingStructModel>((il) =>
            {
                EVar ulongHandler = (ulong)1;
                EVar ulongHandlerFromObject = EVar.CreateVarFromObject((ulong)1);
                EVar stringHandler = " World";
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Load("FieldModel").Set("ValueField", modelHandler.DLoad("FieldModel").DLoad("ValueField").Operator + (ulong)1);
                modelHandler.DLoad("FieldModel").DLoad("ValueField").Operator--;
                modelHandler.DLoad("FieldModel").DLoad("ValueField").Operator++;

                modelHandler.Load("PropertyModel").Set("ValueProperty", modelHandler.DLoad("PropertyModel").DLoad("ValueProperty").Operator + 1);
                modelHandler.DLoad("PropertyModel").DLoad("ValueProperty").Operator--;
                modelHandler.DLoad("PropertyModel").DLoad("ValueProperty").Operator++;

                modelHandler.Load("PropertyModel").Set("RefProperty", modelHandler.DLoad("PropertyModel").DLoad("RefProperty").Operator + " World");

                modelHandler.Load("FieldModel").Set("RefField", modelHandler.DLoad("FieldModel").DLoad("RefField").Operator + stringHandler);
                modelHandler.Load();
            }).Compile();
            Func<NestingStructModel> action = (Func<NestingStructModel>)test;
            NestingStructModel result = action();

            Assert.Equal(ulong.MaxValue, result.FieldModel.ValueField);
            Assert.Equal(ulong.MaxValue, result.PropertyModel.ValueProperty);
            Assert.Equal("Hello World", result.FieldModel.RefField);
            Assert.Equal("Hello World", result.PropertyModel.RefProperty);
            Assert.Equal(ulong.MaxValue, NestingStructModel.Model.FieldModel.ValueField);
        }
        [Fact(DisplayName = "类")]
        public void TestClassOperator()
        {
            NestingClassModel model = new NestingClassModel();
            model.FieldModel = new FieldClass() { RefField = "Hello", ValueField = ulong.MaxValue - 1 };
            model.PropertyModel = new PropertyClass() { RefProperty = "Hello", ValueProperty = ulong.MinValue + 1 };
            model.MethodModel = new MethodClass();
            NestingClassModel.Model = model;

            Delegate test = EHandler.CreateMethod<NestingClassModel>((il) =>
            {
                EVar ulongHandler = (ulong)1;
                EVar ulongHandlerFromObject = EVar.CreateVarFromObject((ulong)1);
                EVar stringHandler = " World";
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Load("FieldModel").Set("ValueField", modelHandler.DLoad("FieldModel").DLoad("ValueField").Operator + (ulong)1);
                modelHandler.DLoad("FieldModel").DLoad("ValueField").Operator--;
                modelHandler.DLoad("FieldModel").DLoad("ValueField").Operator++;

                modelHandler.Load("PropertyModel").Set("ValueProperty", modelHandler.DLoad("PropertyModel").DLoad("ValueProperty").Operator - (ulong)1);
                modelHandler.DLoad("PropertyModel").DLoad("ValueProperty").Operator--;
                modelHandler.DLoad("PropertyModel").DLoad("ValueProperty").Operator++;

                modelHandler.Load("PropertyModel").Set("RefProperty", modelHandler.DLoad("PropertyModel").DLoad("RefProperty").Operator + " World");
                modelHandler.Load("FieldModel").Set("RefField", modelHandler.DLoad("FieldModel").DLoad("RefField").Operator + stringHandler);

                modelHandler.Load();
            }).Compile();
            Func<NestingClassModel> action = (Func<NestingClassModel>)test;
            NestingClassModel result = action();

            Assert.Equal(ulong.MaxValue, result.FieldModel.ValueField);
            Assert.Equal(ulong.MinValue, result.PropertyModel.ValueProperty);
            Assert.Equal("Hello World", result.FieldModel.RefField);
            Assert.Equal("Hello World", result.PropertyModel.RefProperty);
            Assert.Equal(ulong.MaxValue, NestingClassModel.Model.FieldModel.ValueField);
        }
    }



}
