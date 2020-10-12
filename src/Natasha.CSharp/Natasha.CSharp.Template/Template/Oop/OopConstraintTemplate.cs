using Natasha.CSharp.Reverser;
using System;

namespace Natasha.CSharp.Template
{
    public class OopConstraintTemplate<T> : InheritanceTemplate<T> where T : OopConstraintTemplate<T>, new()
    {

        public string ConstraintScript;


        public T ConstraintFrom<TConstraint>()
        {

            ConstraintAppend(GenericConstraintReverser.GetTypeConstraints<TConstraint>());
            return Link;

        }

        public T ConstraintFrom(Type type)
        {

            ConstraintAppend(GenericConstraintReverser.GetTypeConstraints(type));
            return Link;

        }

        public T Constraint(string constraint)
        {
            ConstraintScript = constraint;
            return Link;
        }

        public T ConstraintAppend(string constraint)
        {
            ConstraintScript += constraint;
            return Link;
        }


        public override T BuilderScript()
        {
            // [Attribute]
            // [access] [modifier] [type] [Name][{this}]
            base.BuilderScript();
            if (ConstraintScript != default)
            {
                _script.Append(' ');
                _script.Append(ConstraintScript);
            }
            return Link;
        }
    }
}
