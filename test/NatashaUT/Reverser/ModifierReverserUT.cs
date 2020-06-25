using Natasha.Reverser;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace NatashaUT.Reverser
{

    [Trait("反解器", "修饰符")]
    public class ModifierReverserUT
    {

       

        private void MethodEqual<T>(string key,bool isRight)
        {

            var method = typeof(T).GetMethod("M",
                System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

            var baseMethod = typeof(T).BaseType.GetMethod("M",
                System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

            if (method == baseMethod)
            {
                Assert.Equal(0, 1);
            }
            Assert.Equal(isRight , key + " " == ModifierReverser.GetModifier(method));

            
        }

        [Fact(DisplayName = "虚方法判别")]
        public void TestVirualMethod()
        {

            //MethodEqual<VirtualMethodModel>("virtual", true);
            //MethodEqual<VirtualInterfaceMethodModel>("virtual", true);
            MethodEqual<ImplementMethodModel>("virtual", false);


            MethodEqual<InterfaceMethodModel>("virtual", false);
            MethodEqual<AbstractMethodModel>("virtual", false);
            MethodEqual<OverrideMethodModel>("virtual", false);
            MethodEqual<OverrideVirtualMethodModel>("virtual", false);

            
            MethodEqual<NewMethodModel>("virtual", false);

        }

        public bool IsNewMethod(MethodInfo info)
        {

            var baseInfo = info
                .DeclaringType
                .BaseType
                .GetMethod(info.Name,BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            return info == baseInfo;

        }

        [Fact(DisplayName = "虚方法元数据判别")]
        public void TestMethod1()
        {
            var info = typeof(VirtualMethodModel).GetMethod("M");
            Assert.True(info.IsVirtual);
            Assert.True(info.IsHideBySig);

            Assert.False(info.IsAbstract);
            Assert.False(info.IsFinal);
            Assert.False(IsNewMethod(info));
        }

        [Fact(DisplayName = "虚方法实现接口元数据判别")]
        public void TestMethod2()
        {
            var info = typeof(VirtualInterfaceMethodModel).GetMethod("M");
            Assert.True(info.IsVirtual);
            Assert.True(info.IsHideBySig);

            Assert.False(info.IsAbstract);
            Assert.False(info.IsFinal);
            Assert.False(IsNewMethod(info));
        }


        [Fact(DisplayName = "抽象法元数据判别")]
        public void TestMethod3()
        {
            var info = typeof(AbstractMethodModel).GetMethod("M");
            Assert.True(info.IsVirtual);
            Assert.True(info.IsHideBySig);
            Assert.True(info.IsAbstract);

            Assert.False(info.IsFinal);
            Assert.False(IsNewMethod(info));
        }

        [Fact(DisplayName = "重写方法数据判别")]
        public void TestMetho4()
        {
            var info = typeof(OverrideMethodModel).GetMethod("M");
            Assert.True(info.IsVirtual);
            Assert.True(info.IsHideBySig);
            Assert.True(info.IsAbstract);

            Assert.False(info.IsFinal);
            Assert.False(IsNewMethod(info));
        }
    }
}
