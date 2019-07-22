using System.Reflection;

namespace Natasha
{
    public class OnceModifierTemplate<T>: OnceAccessTemplate<T>
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
            return Link;
        }
        public override T Builder()
        {
            OnceBuilder.Insert(0, OnceModifierScript);
            return base.Builder();
        }
    }
}
