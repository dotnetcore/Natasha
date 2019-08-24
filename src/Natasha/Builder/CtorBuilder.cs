using Natasha.Template;

namespace Natasha.Builder
{
    /// <summary>
    /// 初始化模板
    /// </summary>
    public class CtorBuilder : MethodBodyTemplate<CtorBuilder>
    {
        public CtorBuilder() => Link = this;



        /// <summary>
        /// 使用类模板得访问级别或者修饰符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <returns></returns>
        public CtorBuilder UseTemplate<T>(OopContentTemplate<T> template)
        {

            //类名即方法名
            MethodNameScript = template.OopNameScript;


            //如果是静态的使用静态初始化
            if (template.OopModifierScript == "static ")
            {
                MemberModifierScript = "static ";
            }


            return this;
        }

    }
}
