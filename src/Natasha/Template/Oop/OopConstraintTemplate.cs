using Natasha.Reverser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.Template
{
    public class OopConstraintTemplate<T> : InheritanceTemplate<T> where T : OopConstraintTemplate<T>, new()
    {

        public string ConstraintScript;


        public T Constraint<TConstraint>()
        {
            ConstraintScript = GenericConstraintReverser.GetTypeConstraints<TConstraint>();
            return Link;
        }
        public T Constraint(Type type)
        {
            ConstraintScript = GenericConstraintReverser.GetTypeConstraints(type);
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
