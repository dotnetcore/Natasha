using Natasha.Engine.Reverser;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class MemberModifierTemplate<T>:MemberAccessTemplate<T>
    {
        public string ModifierScript;
        public T Modifier(MethodInfo modifier)
        {
            ModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;
        }
        public T Modifier(Modifiers modifier)
        {
            ModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;
        }
        public T Modifier(string modifier)
        {
            ModifierScript = modifier;
            return Link;
        }
        public override string Builder()
        {
            Script.Insert(0, ModifierScript);
            return base.Builder();
        }
    }
}
