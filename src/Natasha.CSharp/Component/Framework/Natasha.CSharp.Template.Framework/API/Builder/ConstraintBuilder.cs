using Natasha.CSharp.Template.Reverser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.Builder
{
    public class ConstraintBuilder
    {

        private string _typeName;
        private readonly HashSet<Type> _types;
        private readonly HashSet<ConstraintFlags> _enums;

        public ConstraintBuilder()
        {
            _typeName = string.Empty;
            _types = new HashSet<Type>();
            _enums = new HashSet<ConstraintFlags>();
        }

        public ConstraintBuilder SetType(string typeName)
        {
            _typeName = typeName;
            return this;
        }


        public ConstraintBuilder Constraint(ConstraintFlags constraint)
        {

            _enums.Add(constraint);
            return this;

        }

        public ConstraintBuilder Constraint<TConstraint>()
        {

            return Constraint(typeof(TConstraint));

        }
        public ConstraintBuilder Constraint(Type type)
        {

            _types.Add(type);
            return this;

        }

        public string GetScript()
        {

            StringBuilder builder = new StringBuilder();
            if (_types.Count > 0 || _enums.Count > 0)
            {

                
                foreach (var item in _enums)
                {
                    if (item == ConstraintFlags.Class || item == ConstraintFlags.Struct)
                    {

                        builder.Insert(0, GenericConstraintReverser.GetConstraint(item) + ",");

                    }
                    else
                    {

                        builder.Append(GenericConstraintReverser.GetConstraint(item) + ",");

                    }
                    
                }
                foreach (var item in _types)
                {

                    builder.Append(item.GetDevelopName()+",");

                }
                builder.Insert(0, $"where {_typeName} : ");
                builder.Length -= 1;
            }
            return builder.ToString();

        }


    }
}
