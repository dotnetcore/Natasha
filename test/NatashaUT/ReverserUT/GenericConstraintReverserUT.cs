using Natasha.CSharp.Reverser;
using NatashaUT.Model;
using Xunit;

namespace NatashaUT.ReverserUT
{

    [Trait("反解器", "泛型约束反解")]
    public class GenericConstraintReverserUT
    {

        [Fact(DisplayName = "类约束反解")]
        public void Test1()
        {
            var result = GenericConstraintReverser.GetTypeConstraints(typeof(ClassT<>));
            Assert.Equal("where T : notnull, class ", result);
        }

        [Fact(DisplayName = "结构体约束反解")]
        public void Test2()
        {
            var result = GenericConstraintReverser.GetTypeConstraints(typeof(StructT<>));
            Assert.Equal("where T : struct ", result);
        }
        [Fact(DisplayName = "初始化约束反解")]
        public void Test3()
        {
            var result = GenericConstraintReverser.GetTypeConstraints(typeof(NewT<>));
            Assert.Equal("where T : notnull, new() ", result);
        }
        [Fact(DisplayName = "非空约束反解")]
        public void Test9()
        {
            var result = GenericConstraintReverser.GetTypeConstraints(typeof(NotNull<>));
            Assert.Equal("where T : notnull, new() ", result);
        }

        [Fact(DisplayName = "非托管约束反解")]
        public void Test5()
        {
            var result = GenericConstraintReverser.GetTypeConstraints(typeof(UnmanagedT<>));
            Assert.Equal("where T : unmanaged ", result);
        }
        [Fact(DisplayName = "类与初始化约束反解")]
        public void Test4()
        {
            var result = GenericConstraintReverser.GetTypeConstraints(typeof(ClassNewT<>));
            Assert.Equal("where T : notnull, class, new() ", result);
        }

        [Fact(DisplayName = "协变约束反解")]
        public void Test6()
        {
            var result = GenericConstraintReverser.GetVariant(typeof(InInterfaceT<>).GetGenericArguments()[0]);
            Assert.Equal("in T", result);
        }
        [Fact(DisplayName = "抗变约束反解")]
        public void Test7()
        {
            var result = GenericConstraintReverser.GetVariant(typeof(OutInterfaceT<>).GetGenericArguments()[0]);
            Assert.Equal("out T", result);
        }
        [Fact(DisplayName = "复杂约束反解")]
        public void Test8()
        {
            var result = GenericConstraintReverser.GetVariants(typeof(InOutInterfaceT<,>));
            var result1 = GenericConstraintReverser.GetTypeConstraints(typeof(InOutInterfaceT<,>));
            Assert.Equal("in T", result[0]);
            Assert.Equal("out S", result[1]);
            Assert.Equal("where T : NatashaUT.Model.G2, NatashaUT.Model.G3, NatashaUT.Model.G4, new() where S : NatashaUT.Model.G2, NatashaUT.Model.G3, NatashaUT.Model.G4, new() ", result1);
        }



    }
}
