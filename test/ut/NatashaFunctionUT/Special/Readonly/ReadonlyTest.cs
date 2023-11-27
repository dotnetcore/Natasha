using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace NatashaFunctionUT.Special
{
    [Trait("高级API功能测试", "其他")]
    public class ReadonlyTest : DomainPrepare
    {

        [Fact(DisplayName = "私有只读成员动态赋值")]
        public void Test()
        {
            var action = NDelegate
                .RandomDomain(builder => builder.ConfigCompilerOption(opt => opt
                .WithAllMetadata()
                .SetCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility | Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes)))
                .ConfigClass(item => item
                .AllowPrivate<ReadonlyModel>())
                .Action<ReadonlyModel>($"{"obj.@interface".ToReadonlyScript()} = new DefaultReadolyInterface();");

            ReadonlyModel model = new ReadonlyModel();
            action(model);
            Assert.NotNull(model.GetInterface());

        }

        [Fact(DisplayName = "私有成员Unsafe赋值")]
        public void Test2()
        {
            var action = NDelegate
                .RandomDomain(builder => builder.ConfigCompilerOption(opt => opt
                .WithAllMetadata()
                .SetCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility | Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes)))
                .ConfigClass(item => item
                .AllowPrivate<ReadonlyModel>())
                .Action<ReadonlyModel>($"{"obj.@interface2".ToReadonlyScript()} = new DefaultReadolyInterface();");

            ReadonlyModel model = new ReadonlyModel();
            action(model);
            Assert.NotNull(model.GetInterface2());

        }

    }

    public class ReadonlyModel
    {
        public readonly IReadonlyInterface @interface3;
        private readonly IReadonlyInterface @interface;
        private IReadonlyInterface @interface2;
        public IReadonlyInterface GetInterface()
        {
            return @interface;
        }
        public IReadonlyInterface GetInterface2()
        {
            return @interface2;
        }
    }

    public interface IReadonlyInterface { }

    public class DefaultReadolyInterface : IReadonlyInterface { }
}
