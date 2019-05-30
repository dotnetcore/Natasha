namespace Natasha
{
    public class CtorTemplate: MethodContentTemplate<CtorTemplate>,IScriptBuilder
    {
        public CtorTemplate()
        {
            Link = this;
        }

        public CtorTemplate UseTemplate<T>(ClassContentTemplate<T> template)
        {
            NameScript = template.NameScript;
            if (template.ModifierScript=="static ")
            {
                ModifierScript = "static ";
            }
            else
            {
                AccessScript = template.AccessScript;
            }
            return this;
        }
    }
}
