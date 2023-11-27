using Xunit;

namespace FrameworkFunctionUT.Special
{
    [Trait("高级API功能测试", "其他")]
    public class ReadonlyTest : DomainPrepare
    {


        [Fact(DisplayName = "私有只读成员动态赋值")]
        public void Test()
        {

            var action = NDelegate
                .DefaultDomain(builder => builder.ConfigCompilerOption(opt => opt
                .WithAllMetadata()
                .SetCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility | Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes)))
                .ConfigClass(item => item.AllowPrivate<ReadonlyModel>())
                .Action<ReadonlyModel>($"{"obj.@interface".ToReadonlyScript()} = new DefaultReadolyInterface();");

            ReadonlyModel model = new ReadonlyModel();
            action(model);
            
            Assert.NotNull(model.GetInterface());

        }

    }

    public class ReadonlyModel
    {
        private readonly IReadonlyInterface @interface;

        public IReadonlyInterface GetInterface()
        {
            return @interface;
        }

    }

    public interface IReadonlyInterface { }

    public class DefaultReadolyInterface : IReadonlyInterface { }
}
