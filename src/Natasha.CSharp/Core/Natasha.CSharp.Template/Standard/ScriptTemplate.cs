namespace Natasha.CSharp.Template
{
    public class ScriptTemplate<T> : CompilerTemplate<T> where T : ScriptTemplate<T>, new()
    {
        public string ScriptWrapper(string script)
        {
            if (script != string.Empty && !script.EndsWith(" "))
            {
                script += " ";
            }
            return script;
        }
        public virtual string GetScript()
        {
            return string.Empty;
        }
        public virtual T BuilderScript() { return Link; }
    }
}
