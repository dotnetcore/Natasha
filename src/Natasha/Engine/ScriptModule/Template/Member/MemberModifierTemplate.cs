using System.Reflection;

namespace Natasha
{
    public class MemberModifierTemplate<T>:MemberAccessTemplate<T>
    {
        public string ModifierScript;
        public T MemberModifier(MethodInfo modifier)
        {
            ModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;
        }
        public T MemberModifier(Modifiers modifier)
        {
            ModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;
        }
        public T MemberModifier(string modifier)
        {
            ModifierScript = modifier;
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
