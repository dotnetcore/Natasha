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
    [Trait("赋值/加载", "特殊类")]
    public class ClassWithNullableTest
    {
        [Fact(DisplayName = "属性")]
        public void TestProperty()
        {
            ClassWithNullableModel model = new ClassWithNullableModel();
            model.ValueField = 100;
            model.ValueProperty = null;
            ClassWithNullableModel.StaticValueField = null;
            ClassWithNullableModel.StaticValueProperty = 200;
            Delegate test = EHandler.CreateMethod<ClassWithNullableModel>((il) =>
            {
                EModel modelHandler = EModel.CreateModelFromObject(model);

                modelHandler.Set("ValueProperty", modelHandler.DLoad("PrivateProperty").DLoad("Value").Operator + modelHandler.DLoad("StaticValueProperty").DLoad("Value").Operator);
                modelHandler.Set("ValueProperty", modelHandler.DLoad("PrivateProperty").DLoad("Value").Operator + modelHandler.DLoad("StaticValueProperty").DLoad("Value").Operator);
                modelHandler.Set("StaticValueProperty", modelHandler.DLoad("ValueProperty").DLoad("Value").Operator + modelHandler.DLoad("StaticValueProperty").DLoad("Value").Operator);
                modelHandler.Set("ValueField", modelHandler.DLoad("StaticValueProperty").DLoad("Value").Operator + modelHandler.DLoad("ValueField").DLoad("Value").Operator);
                modelHandler.Load();
            }).Compile();
            Func<ClassWithNullableModel> action = (Func<ClassWithNullableModel>)test;
            ClassWithNullableModel result = action();
            Assert.Equal((ulong)200, result.ValueProperty);
            Assert.Equal((ulong)400, ClassWithNullableModel.StaticValueProperty);
            Assert.Equal((ulong)500, result.ValueField);
        }
    }
}
