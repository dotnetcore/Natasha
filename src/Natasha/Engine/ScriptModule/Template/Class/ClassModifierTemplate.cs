using System.Reflection;

namespace Natasha.Template
{
    public class ClassModifierTemplate<T> : ClassAccessTemplate<T>
    {

        public string ClassModifierScript;


        public T ClassModifier(MethodInfo reflectMethodInfo)
        {

            ClassModifierScript = ModifierReverser.GetModifier(reflectMethodInfo);
            return Link;

        }




        public T ClassModifier(Modifiers enumModifier)
        {

            ClassModifierScript = ModifierReverser.GetModifier(enumModifier);
            return Link;

        }




        public T ClassModifier(string modifier)
        {

            ClassModifierScript = modifier + " ";
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(ClassModifierScript);
            return Link;

        }

    }

}
