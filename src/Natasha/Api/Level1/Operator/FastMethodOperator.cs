using Natasha.Builder;

namespace Natasha
{
    /// <summary>
    /// 快速创建一个动态方法
    /// </summary>
    public class FastMethodOperator : MethodBuilder<FastMethodOperator>
    {

        public FastMethodOperator()
        {
            Link = this;
            this.Access(Reverser.Model.AccessTypes.Public)
                .Modifier(Reverser.Model.Modifiers.Static);
        }




        public override void Init()
        {

            ClassOptions(item => item
            .Modifier(Reverser.Model.Modifiers.Static)
            .Class()
            .UseRandomName()
            .HiddenNamespace()
            .Access(Reverser.Model.AccessTypes.Public)
            );

        }


    }

}
