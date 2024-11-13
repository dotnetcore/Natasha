using Natasha.CSharp.Template.Reverser;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace NatashaFunctionUT.Reverser
{
    [Trait("基础功能测试", "反解器")]
    public class ModifierReverserTest
    {
        public static readonly BindingFlags ReflectBindFlag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance;
        
        
        private static void Test(string modifier)
        {
            var eventInfo = typeof(ModifierReverserTestModel).GetEvent(modifier + "Event", ReflectBindFlag);
            var fieldInfo = typeof(ModifierReverserTestModel).GetField(modifier + "Field", ReflectBindFlag);
            var propertyInfo = typeof(ModifierReverserTestModel).GetProperty(modifier + "Property", ReflectBindFlag);
            var methodInfo = typeof(ModifierReverserTestModel).GetMethod(modifier + "Method", ReflectBindFlag);

            if (eventInfo!=null)
            {
                Assert.Equal(modifier, ModifierReverser.GetModifier(eventInfo).Replace(" ", ""));
            }
            if (propertyInfo != null)
            {
                Assert.Equal(modifier, ModifierReverser.GetModifier(propertyInfo).Replace(" ", ""));
            }
            if (methodInfo != null)
            {
                Assert.Equal(modifier, ModifierReverser.GetModifier(methodInfo).Replace(" ", ""));
            }
            if (fieldInfo != null)
            {
                Assert.Equal(modifier, ModifierReverser.GetModifier(fieldInfo).Replace(" ", ""));
            }
        }


        [Fact(DisplayName ="抽象成员测试")]
        public void AbstractTest()
        {
            Test("abstract");
        }


        [Fact(DisplayName = "虚成员测试")]
        public void VirtualcTest()
        {
            Test("virtual");
        }


        [Fact(DisplayName = "新成员测试")]
        public void NewTest()
        {
            Test("new");
        }


        [Fact(DisplayName = "重写成员测试")]
        public void OverrideTest()
        {
            Test("override");
        }

        [Fact(DisplayName = "重写异步成员测试")]
        public void OverrideAsyncTest()
        {
            Test("overrideasync");
        }


        [Fact(DisplayName = "异步成员测试")]
        public void AsyncTest()
        {
            Test("async");
        }

        [Fact(DisplayName = "异步虚成员测试")]
        public void VirtualAsyncTest()
        {
            Test("virtualasync");
        }
        public abstract class ModifierReverserTestModel : ModifierReverserTestBaseModel
        {

            public virtual event Action? virtualEvent;
            public virtual string? virtualProperty { get; set; }
            public virtual string? virtualMethod() { return String.Empty; }

            public abstract event Action? abstractEvent;
            public abstract string? abstractProperty { get; set; }
            public abstract string? abstractMethod();
            public async Task asyncMethod()
            {
                await Task.Delay(1000);
            }
            public virtual async Task virtualasyncMethod()
            {
                await Task.Delay(1000);
            }
            public override async Task<string> overrideasyncMethod()
            {
                await Task.Delay(10);
                throw new NotImplementedException();
            }
            public override string overrideMethod()
            {
                throw new NotImplementedException();
            }
            public new event Action? newEvent;
            public new string? newField;
            public new string? newProperty { get; set; }
            public new string? newMethod() { return null; }
        }

        public abstract class ModifierReverserTestBaseModel
        {
            public event Action? newEvent;
            public string? newField;
            public string? newProperty { get; set; }
            public string? newMethod() { return null; }
            public abstract Task<string> overrideasyncMethod();
            public abstract string overrideMethod();
        }

    }
}
