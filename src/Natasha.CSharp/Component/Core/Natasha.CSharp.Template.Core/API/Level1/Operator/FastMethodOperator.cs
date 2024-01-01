using Natasha.CSharp.Builder;
using Natasha.CSharp.Template;

namespace Natasha.CSharp
{
    /// <summary>
    /// 快速创建一个动态方法
    /// </summary>
    public sealed class FastMethodOperator : MethodBuilder<FastMethodOperator>
    {

        public FastMethodOperator()
        {
            Link = this;
            this.Access(AccessFlags.Public)
                .Modifier(ModifierFlags.Static)
                .UseRandomName();
        }




        public override void Init()
        {

            ClassOptions(item => item
            .Modifier(ModifierFlags.Static)
            .Class()
            .UseRandomName()
            .HiddenNamespace()
            .Access(AccessFlags.Public)
            );

        }


    }

}
