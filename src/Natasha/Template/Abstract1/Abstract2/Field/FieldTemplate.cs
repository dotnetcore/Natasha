namespace Natasha.Template
{

    public class FieldTemplate<T> : DefinedNameTemplate<T> where T : FieldTemplate<T>, new()
    {

        public override T Builder()
        {

            // [Attribute]
            // [access] [modifier] [type] [Name][{this}]  
            base.Builder();
            _script.Append(';');
            return Link;

        }

    }

}
