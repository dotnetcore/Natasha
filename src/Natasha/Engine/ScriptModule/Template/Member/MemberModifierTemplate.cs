using System.Reflection;

namespace Natasha.Template
{

    public class MemberModifierTemplate<T>:MemberAccessTemplate<T>
    {

        public string MemberModifierScript;


        public T MemberModifier(MethodInfo modifier)
        {

            MemberModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;

        }




        public T MemberModifier(Modifiers modifier)
        {

            MemberModifierScript = ModifierReverser.GetModifier(modifier);
            return Link;

        }




        public T MemberModifier(string modifier)
        {

            MemberModifierScript = modifier;
            if (!MemberModifierScript.EndsWith(" "))
            {
                MemberModifierScript += " ";
            }
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(MemberModifierScript);
            return Link;

        }

    }

}
