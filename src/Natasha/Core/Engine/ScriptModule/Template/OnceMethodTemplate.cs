namespace Natasha.Template
{
    /// <summary>
    /// 快速构建一个方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OnceMethodTemplate<T> : OnceMethodDelegateTemplate<T>
    {

        public override T Builder()
        {

            _script.Clear();
            return base.Builder();

        }

    }
}
