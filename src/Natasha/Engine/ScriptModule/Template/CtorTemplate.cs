namespace Natasha.Template
{
    /// <summary>
    /// 初始化模板
    /// </summary>
    public class CtorTemplate: MethodBodyTemplate<CtorTemplate>
    {
        public CtorTemplate() => Link = this;



        /// <summary>
        /// 使用类模板得访问级别或者修饰符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <returns></returns>
        public CtorTemplate UseTemplate<T>(ClassContentTemplate<T> template)
        {

            //类名即方法名
            MethodNameScript = template.ClassNameScript;


            //如果是静态的使用静态初始化
            if (template.ClassModifierScript=="static ")
            {
                MemberModifierScript = "static ";
            }
            else
            {
                MemberAccessScript = template.ClassAccessScript;
            }


            return this;
        }




        public override CtorTemplate Builder()
        {

            _script.Clear();
            return base.Builder();

        }

    }
}
