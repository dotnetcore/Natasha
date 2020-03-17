using System;
using System.Reflection;
using Natasha.Reverser;
using Natasha.Reverser.Model;

namespace Natasha.Template
{

    public class MemberModifierTemplate<T> : MemberAccessTemplate<T> where T : MemberModifierTemplate<T>, new()
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




        public T StaticMember
        {
            get { MemberModifierScript = "static "; return Link; }
        }
        public T AbstractMember
        {
            get { MemberModifierScript = "abstract "; return Link; }
        }
        public T NewMember
        {
            get { MemberModifierScript = "new "; return Link; }
        }
        public T VirtualMember
        {
            get { MemberModifierScript = "virtual "; return Link; }
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
