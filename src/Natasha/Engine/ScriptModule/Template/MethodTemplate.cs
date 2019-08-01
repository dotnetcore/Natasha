namespace Natasha.Template
{
    /// <summary>
    /// 方法模板
    /// </summary>
    public class MethodTemplate : MethodDelegateTemplate<MethodTemplate>
    {
        public MethodTemplate() => Link = this;

        public override MethodTemplate Builder()
        {

            _script.Clear();
            return base.Builder();

        }

    }
}
