namespace Natasha.CSharp.Template
{

    public class FieldTemplate<T> : DefinedNameTemplate<T> where T : FieldTemplate<T>, new()
    {

        public override T BuilderScript()
        {
            // [comment]
            // [attribute]
            // [access] [modifier] [type] [name][{this}]  
            base.BuilderScript();
            _script.Append(';');
            return Link;

        }

    }

}
