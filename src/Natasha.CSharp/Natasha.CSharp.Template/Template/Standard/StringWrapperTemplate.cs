using Natasha.CSharp.Template;

namespace Natasha.Template
{
    public class StringWrapperTemplate<T> : CompilerTemplate<T> where T : StringWrapperTemplate<T>, new()
    {

        public string ScriptWrapper(string script)
        {
            if (script != default && !script.EndsWith(" "))
            {
                script += " ";
            }
            return script;
        }

    }
}
