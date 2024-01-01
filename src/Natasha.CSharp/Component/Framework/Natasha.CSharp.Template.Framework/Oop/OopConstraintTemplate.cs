using System;
using Natasha.CSharp.Template.Reverser;

namespace Natasha.CSharp.Template
{
    public class OopConstraintTemplate<T> : InheritanceTemplate<T> where T : OopConstraintTemplate<T>, new()
    {

        public string ConstraintScript;

        public OopConstraintTemplate()
        {
            ConstraintScript = string.Empty;
        }

        public T ConstraintFrom<TConstraint>()
        {

            Constraint(GenericConstraintReverser.GetTypeConstraints<TConstraint>());
            return Link;

        }

        public T ConstraintFrom(Type type)
        {

            Constraint(GenericConstraintReverser.GetTypeConstraints(type));
            return Link;

        }

        public T ConstraintAppendFrom<TConstraint>()
        {

            ConstraintAppend(GenericConstraintReverser.GetTypeConstraints<TConstraint>());
            return Link;

        }

        public T ConstraintAppendFrom(Type type)
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
            // [comment]
            // [attribute]
            // [access] [modifier] [type] [name][{this}]
            base.BuilderScript();
            if (ConstraintScript != string.Empty)
            {
                _script.Append(' ');
                _script.Append(ConstraintScript);
            }
            return Link;
        }
    }
}
