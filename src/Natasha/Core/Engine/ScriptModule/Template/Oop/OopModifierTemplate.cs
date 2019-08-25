using System.Reflection;

namespace Natasha.Template
{
    public class OopModifierTemplate<T> : OopAccessTemplate<T>
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




        public T ClassModifier(string modifier)
        {

            OopModifierScript = modifier + " ";
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
