using System.Reflection;

namespace Natasha.Template
{
    public class OnceMethodModifierTemplate<T>: OnceMethodAccessTemplate<T>
    {

        public string OnceModifierScript;
        public T MethodModifier(MethodInfo modifier)
        {

            OnceModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;

        }




        public T MethodModifier(Modifiers modifier)
        {

            OnceModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;

        }




        public T MethodModifier(string modifier)
        {

            OnceModifierScript = modifier;
            if (OnceAccessScript.EndsWith(" "))
            {
                OnceAccessScript += " ";
            }
            return Link;

        }




        public override T Builder()
        {

            OnceBuilder.Insert(0, OnceModifierScript);
            return base.Builder();

        }

    }

}
