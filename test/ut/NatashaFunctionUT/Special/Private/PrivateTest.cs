using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace NatashaFunctionUT.Special
{
    [Trait("高级API功能测试", "其他")]
    public class PrivateTest : DomainPrepare
    {

        [Fact(DisplayName = "私有成员调用")]
        public void Test()
        {
            PrivateMemberClassModel test = new PrivateMemberClassModel();
            var action = NDelegate
                .RandomDomain(builder => builder
                    .UseSmartMode()
                    .ConfigCompilerOption(opt => opt
                        .WithAllMetadata()
                        .WithCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility | Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes)
                        ))
                .ConfigClass(item => item.AllowPrivate<PrivateMemberClassModel>())
                .Action<PrivateMemberClassModel>("obj.publicA = 2;obj.internalA = 3; obj.privateA=1;");
            action(test);
            Assert.Equal(4, test.GetD);


        }


        [Fact(DisplayName = "私有成员调用2")]
        public void Test1()
        {

            PrivateMemberClassModel test = new PrivateMemberClassModel();
            var action = NDelegate
                .RandomDomain(builder => builder.UseSmartMode().ConfigCompilerOption(opt => opt
                    .WithAllMetadata()
                    .WithCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility | Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes)))
                .ConfigClass(item => item.AllowPrivate<PrivateMemberClassModel>())
                .Action<PrivateMemberClassModel>("obj.publicA = 2;obj.internalA = 3; obj.privateA=1;");
            action(test);
            Assert.Equal(4, test.GetD);

        }
        [Fact(DisplayName = "私有成员调用3")]
        public void Test2()
        {
            PrivateMemberClassModel test = new PrivateMemberClassModel();
            var action = NDelegate
                .RandomDomain(builder => 
                {
                    builder
                    .UseSimpleMode()
                    .AddReferenceAndUsingCode(typeof(List<int>))
                    .AddReferenceAndUsingCode(typeof(object))
                    .AddReferenceAndUsingCode(typeof(System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute))
                    .ConfigCompilerOption(opt => opt
                        .WithAllMetadata()
                        .WithCompilerFlag(Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreAccessibility | Natasha.CSharp.Compiler.CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes));

                    var assemblyNames = typeof(System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute).Assembly.GetReferencedAssemblies();
                    foreach (var name in assemblyNames)
                    {
                        var assmebly = Assembly.Load(name);
                        builder.AddReferenceAndUsingCode(assmebly, AssemblyCompareInfomation.UseForce);
                    }

                })
                .ConfigClass(item => item.AllowPrivate<List<int>>())
                .Func<int[]>("return (new List<int>() { 1 })._items;");
            var array = action();
            Assert.NotNull(array);
        }

    }


    public class PrivateMemberClassModel
    {
        public int publicA { get; set; }
        protected int protectedA { get; set; }
        internal int internalA { get; set; }
        private int privateA { get; set; }
        protected internal int E { get; set; }
        internal protected int F { get; set; }
        public int GetD { get { return privateA + internalA; } }
    }
}
