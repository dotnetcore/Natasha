using Natasha.Builder;

namespace Natasha.Operator
{

    /// <summary>
    /// 类构建器
    /// </summary>
    public class OopOperator : OopBuilder<OopOperator>
    {
        
        public OopOperator()
        {
            Link = this;
        }

    }
}
