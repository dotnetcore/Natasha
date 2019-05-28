using Natasha.Engine.Reverser;
using System.Reflection;

namespace Natasha
{
    public class ClassModifierTemplate<T> : ClassAccessTemplate<T>
    {
        public string ModifierScript;
        public T ClassModifier(MethodInfo modifier)
        {
            ModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;
        }
        public T ClassModifier(Modifiers modifier)
        {
            ModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;
        }
        public T ClassModifier(string modifier)
        {
            ModifierScript = modifier + " ";
            return Link;
        }

        public override string Builder()
        {
            Script.Insert(0, ModifierScript);
            return base.Builder();
        }
    }
}
