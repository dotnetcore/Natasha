using Natasha.CSharp.Template.Reverser;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace FrameworkFunctionUT.Reverser
{
    [Trait("基础功能测试", "反解器")]
    public class CanOvverideMethodReverserTest
    {
        public static readonly BindingFlags ReflectBindFlag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance;
        
        
        private static void Test(string modifier)
        {
            var eventInfo = typeof(OverrideReverserTestModel).GetEvent(modifier + "Event", ReflectBindFlag);
            var propertyInfo = typeof(OverrideReverserTestModel).GetProperty(modifier + "Property", ReflectBindFlag);
            var methodInfo = typeof(OverrideReverserTestModel).GetMethod(modifier + "Method", ReflectBindFlag);

            modifier = modifier.Replace("base", "").Replace("deep","");
            if (eventInfo!=null)
            {
                
                Assert.Equal(modifier, ModifierReverser.GetCanOverrideModifier(eventInfo)!.Replace(" ",""));
            }
            if (propertyInfo != null)
            {
                Assert.Equal(modifier, ModifierReverser.GetCanOverrideModifier(propertyInfo)!.Replace(" ", ""));
            }
            if (methodInfo != null)
            {
                Assert.Equal(modifier, ModifierReverser.GetCanOverrideModifier(methodInfo)!.Replace(" ", ""));
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


        [Fact(DisplayName = "重写抽象成员测试")]
        public void OverrideTest()
        {
            Test("overridebase");
        }

        [Fact(DisplayName = "重写抽象异步成员测试")]
        public void OverrideAsyncTest()
        {
            Test("overrideasyncbase");
        }

        [Fact(DisplayName = "重写深度抽象异步成员测试")]
        public void OverrideDeepthTest()
        {
            Test("deepoverride");
        }
        [Fact(DisplayName = "重写深度虚成员测试")]
        public void OverrideDeepthVirtualTest()
        {
            Test("deepoverridebase");
        }


        public abstract class OverrideReverserTestModel : OverrideReverserTestBaseModel2, IOverrideReverserInterfaceModel
        {

            public virtual event Action? virtualEvent;
            public virtual string? virtualProperty { get; set; }
            public virtual string? virtualMethod() { return String.Empty; }

            public abstract event Action? abstractEvent;
            public abstract string? abstractProperty { get; set; }
            public abstract string? abstractMethod();

            public virtual async Task virtualAsyncMethod()
            {
                await Task.Delay(1000);
            }

            public override void deepoverridebaseMethod()
            {
                base.deepoverridebaseMethod();
            }
            public override void deepoverrideMethod()
            {
                throw new NotImplementedException();
            }
            public override async Task<string> overrideasyncbaseMethod()
            {
                await Task.Delay(10);
                throw new NotImplementedException();
            }
            public override string overridebaseMethod()
            {
                throw new NotImplementedException();
            }

            public override string virtualbaseMethod()
            {
                return base.virtualbaseMethod();
            }

            public void interfaceMethod()
            {
                throw new NotImplementedException();
            }
        }

        public abstract class OverrideReverserTestBaseModel
        {
            public abstract void deepoverrideMethod();
            public virtual void deepoverridebaseMethod()
            {

            }
        }


        public abstract class OverrideReverserTestBaseModel2 : OverrideReverserTestBaseModel
        {
            public event Action? newEvent;
            public string? newField;
            public string? newProperty { get; set; }
            public string? newMethod() { return null; }
            public abstract Task<string> overrideasyncbaseMethod();
            public abstract string overridebaseMethod();

            public virtual string virtualbaseMethod()
            {
                return string.Empty;
            }

            public override void deepoverrideMethod()
            {
                throw new NotImplementedException();
            }

            public override void deepoverridebaseMethod()
            {
                base.deepoverridebaseMethod();
            }
        }

        public interface IOverrideReverserInterfaceModel
        {
            public void interfaceMethod();
        }

    }
}
