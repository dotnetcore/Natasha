namespace Natasha.CSharp.Template
{

    public class FieldTemplate<T> : DefinedNameTemplate<T> where T : FieldTemplate<T>, new()
    {

        public override T BuilderScript()
        {

            // [Attribute]
            // [access] [modifier] [type] [Name][{this}]  
            base.BuilderScript();
            _script.Append(';');
            return Link;

        }

    }

}
