namespace Natasha.Template
{
    public class FieldTemplate<T>: FieldNameTemplate<T>
    {

        public override T Builder()
        {
            _script.Clear();
            base.Builder();
            return Link;

        }

    }

}
