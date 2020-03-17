using Natasha.Reverser;
using Natasha.Reverser.Model;
using System;
using System.Reflection;

namespace Natasha.Template
{
    public class OopModifierTemplate<T> : OopAccessTemplate<T> where T : OopModifierTemplate<T>, new()
    {

        public string OopModifierScript;


        public T OopModifier(MethodInfo reflectMethodInfo)
        {

            OopModifierScript = ModifierReverser.GetModifier(reflectMethodInfo);
            return Link;

        }



        public T OopModifier(Modifiers enumModifier)
        {

            OopModifierScript = ModifierReverser.GetModifier(enumModifier);
            return Link;

        }



        public T Sealed
        {
            get { OopModifierScript = "sealed ";return Link; }
        }
        public T Static
        {
            get { OopModifierScript = "static "; return Link; }
        }
        public T Abstract
        {
            get { OopModifierScript = "abstract "; return Link; }
        }
        public T Partial
        {
            get { OopModifierScript = "partial "; return Link; }
        }
        public T PartialAbstract
        {
            get { OopModifierScript = "partial abstract "; return Link; }
        }




        public T OopModifier(string modifier)
        {

            OopModifierScript = modifier + " ";
            return Link;

        }




        public T OopModifier(Type type)
        {

            OopModifierScript = ModifierReverser.GetModifier(type);
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(OopModifierScript);
            return Link;

        }

    }

}
