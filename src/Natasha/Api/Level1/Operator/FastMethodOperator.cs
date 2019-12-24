using Natasha.Builder;

namespace Natasha.Operator
{
    /// <summary>
    /// 快速创建一个动态方法
    /// </summary>
    public class FastMethodOperator : OnceMethodBuilder<FastMethodOperator>
    {


        public FastMethodOperator()
        {

            Link = this;
            Public
                .Static
                .PublicMember
                .StaticMember
                .UseRandomOopName()
                .HiddenNameSpace();

        }



        public override T Complie<T>(object binder = null)
        {

            var method = typeof(T).GetMethod("Invoke");
            if (ParametersMappings.Count == 0)
            {

                Param(method);

            }


            if (ReturnScript == default)
            {

                Return(method.ReturnType);

            }
            return base.Complie<T>(binder);

        }

    }

}
