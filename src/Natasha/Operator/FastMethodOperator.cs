using Natasha.Builder;

namespace Natasha
{
    /// <summary>
    /// 快速创建一个动态方法
    /// </summary>
    public class FastMethodOperator : OnceMethodBuilder<FastMethodOperator>
    {

        public static FastMethodOperator New
        {
            get
            {

                return new FastMethodOperator();

            }
        }




        public FastMethodOperator()
        {

            Link = this;
            HiddenNameSpace()
                .OopAccess(AccessTypes.Public)
                .OopModifier(Modifiers.Static)
                .MethodAccess(AccessTypes.Public)
                .MethodModifier(Modifiers.Static);

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
