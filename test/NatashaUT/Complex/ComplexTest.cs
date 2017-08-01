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
    [Trait("赋值/加载", "复杂嵌套类型")]
    public class ComplexTest
    {
        [Fact(DisplayName = "结构体包含类")]
        public void TestStruct()
        {
            ComplexStructModel model = new ComplexStructModel();
            model.FieldModel = new FieldClass();
            model.PropertyModel = new PropertyClass();
            model.MethodModel = new MethodClass();
            ComplexStructModel.Model = model;

            Delegate test = EHandler.CreateMethod<ComplexStructModel>((il) =>
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
            Func<ComplexStructModel> action = (Func<ComplexStructModel>)test;
            ComplexStructModel result = action();

            Assert.Equal((ulong)0, result.FieldModel.ValueField);
            Assert.Equal(ulong.MinValue, result.PropertyModel.ValueProperty);
            Assert.Equal("Hello", result.FieldModel.RefField);
            Assert.Equal("Hello", result.PropertyModel.RefProperty);
            Assert.Equal(ulong.MaxValue, ComplexStructModel.Model.FieldModel.ValueField);
            Assert.Equal("Hello1", ComplexStructModel.Model.FieldModel.RefField);
            //Assert.Equal("Hello1", model.FieldModel.RefField);
        }
        [Fact(DisplayName = "类包含结构体")]
        public void TestClass()
        {
            ComplexClassModel model = new ComplexClassModel();
            model.FieldModel = new FieldStruct();
            model.PropertyModel = new PropertyStruct();
            model.MethodModel = new MethodStruct();
            ComplexClassModel.Model = model;

            Delegate test = EHandler.CreateMethod<ComplexClassModel>((il) =>
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
            Func<ComplexClassModel> action = (Func<ComplexClassModel>)test;
            ComplexClassModel result = action();

            Assert.Equal((ulong)0, result.FieldModel.ValueField);
            Assert.Equal(ulong.MinValue, result.PropertyModel.ValueProperty);
            Assert.Equal("Hello", result.FieldModel.RefField);
            Assert.Equal("Hello", result.PropertyModel.RefProperty);
            Assert.Equal(ulong.MaxValue, ComplexClassModel.Model.FieldModel.ValueField);
            Assert.Equal("Hello1", ComplexClassModel.Model.FieldModel.RefField);
            //Assert.Equal("Hello1", model.FieldModel.RefField);
        }
    }
    [Trait("运算", "单纯嵌套结构")]
    public class ComplexOperatorTest
    {
        [Fact(DisplayName = "结构体")]
        public void SOTest()
        {
            ComplexStructModel model = new ComplexStructModel();
            model.FieldModel = new FieldClass() { RefField = "Hello", ValueField = ulong.MaxValue - 1 };
            model.PropertyModel = new PropertyClass() { RefProperty = "Hello", ValueProperty = ulong.MinValue + 1 };
            model.MethodModel = new MethodClass();
            ComplexStructModel.Model = model;

            Delegate test = EHandler.CreateMethod<ComplexStructModel>((il) =>
            {
                EVar ulongHandler = (ulong)1;
                EVar ulongHandlerFromObject = EVar.CreateVarFromObject((ulong)1);
                EVar stringHandler = " World";
                EModel modelHandler = EModel.CreateModelFromObject(model);
                modelHandler.Load("FieldModel").Set("ValueField", modelHandler.DLoad("FieldModel").DLoad("ValueField").Operator + (ulong)1);
                modelHandler.DLoad("FieldModel").DLoad("ValueField").Operator--;
                modelHandler.DLoad("FieldModel").DLoad("ValueField").Operator++;

                modelHandler.Load("PropertyModel").Set("ValueProperty", modelHandler.DLoad("PropertyModel").DLoad("ValueProperty").Operator - (ulong)1);
                modelHandler.DLoad("PropertyModel").DLoad("ValueProperty").Operator++;
                modelHandler.DLoad("PropertyModel").DLoad("ValueProperty").Operator--;

                modelHandler.Load("PropertyModel").Set("RefProperty", modelHandler.DLoad("PropertyModel").DLoad("RefProperty").Operator + " World");

                modelHandler.Load("FieldModel").Set("RefField", modelHandler.DLoad("FieldModel").DLoad("RefField").Operator + stringHandler);
                modelHandler.DLoad("Model").DLoad("FieldModel").DLoad("ValueField").Operator++;
                modelHandler.Load();
            }).Compile();
            Func<ComplexStructModel> action = (Func<ComplexStructModel>)test;
            ComplexStructModel result = action();

            Assert.Equal(ulong.MaxValue, result.FieldModel.ValueField);
            Assert.Equal(ulong.MinValue, result.PropertyModel.ValueProperty);
            Assert.Equal("Hello World", result.FieldModel.RefField);
            Assert.Equal("Hello World", result.PropertyModel.RefProperty);
            Assert.Equal(ulong.MaxValue, ComplexStructModel.Model.FieldModel.ValueField);
        }
        [Fact(DisplayName = "类")]
        public void COTest()
        {
            ComplexClassModel model = new ComplexClassModel();
            model.FieldModel = new FieldStruct() { RefField = "Hello", ValueField = ulong.MaxValue - 1 };
            model.PropertyModel = new PropertyStruct() { RefProperty = "Hello", ValueProperty = ulong.MinValue + 1 };
            model.MethodModel = new MethodStruct();
            ComplexClassModel.Model = model;

            Delegate test = EHandler.CreateMethod<ComplexClassModel>((il) =>
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
            Func<ComplexClassModel> action = (Func<ComplexClassModel>)test;
            ComplexClassModel result = action();

            Assert.Equal(ulong.MaxValue, result.FieldModel.ValueField);
            Assert.Equal(ulong.MinValue, result.PropertyModel.ValueProperty);
            Assert.Equal("Hello World", result.FieldModel.RefField);
            Assert.Equal("Hello World", result.PropertyModel.RefProperty);
            Assert.Equal(ulong.MaxValue, ComplexClassModel.Model.FieldModel.ValueField);
        }
    }
}
