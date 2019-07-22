using System.Reflection;

namespace Natasha
{
    public class ClassModifierTemplate<T> : ClassAccessTemplate<T>
    {
        public string ModifierScript;
        public T ClassModifier(MethodInfo reflectMethodInfo)
        {
            ModifierScript = ModifierReverser.GetModifier(reflectMethodInfo);
            return Link;
        }
        public T ClassModifier(Modifiers enumModifier)
        {
            ModifierScript = ModifierReverser.GetModifier(enumModifier);
            return Link;
        }
        public T ClassModifier(string modifier)
        {
            ModifierScript = modifier + " ";
            return Link;
        }

        public override T Builder()
        {
            base.Builder();
            _script.Append(ModifierScript);
            return Link;
        }
    }
}
