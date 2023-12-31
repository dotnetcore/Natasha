namespace Natasha.CSharp.Template
{
    public class DefinedNullableTemplate<T> : DefinedTypeTemplate<T> where T : DefinedNameTemplate<T>, new()
    {

        public bool NullableFlag;
        public T Nullable()
        {

            NullableFlag = true;
            return Link;

        }

        public T NotNullable()
        {

            NullableFlag = false;
            return Link;

        }

        public override T BuilderScript()
        {
            // [comment]
            // [attribute]
            // [access] [modifier] [type][{this}] [name]{}
            base.BuilderScript();
            if (NullableFlag)
            {
               _script.Append('?');
            }
            return Link;
        }

    }
}
