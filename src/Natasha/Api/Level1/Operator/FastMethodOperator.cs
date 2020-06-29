using Natasha.CSharp.Builder;

namespace Natasha.CSharp
{
    /// <summary>
    /// 快速创建一个动态方法
    /// </summary>
    public class FastMethodOperator : MethodBuilder<FastMethodOperator>
    {

        public FastMethodOperator()
        {
            Link = this;
            this.Access(Reverser.Model.AccessFlags.Public)
                .Modifier(Reverser.Model.ModifierFlags.Static);
        }




        public override void Init()
        {

            ClassOptions(item => item
            .Modifier(Reverser.Model.ModifierFlags.Static)
            .Class()
            .UseRandomName()
            .HiddenNamespace()
            .Access(Reverser.Model.AccessFlags.Public)
            );

        }


    }

}
