using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public abstract class DynamicBase
    {
        public string _current_name;
        public DynamicBase this[string name]
        {
            get {
                _current_name = name;
                return this;
            }
        }


        public abstract T Get<T>(string name);

        public abstract T Get<T>();

        public abstract void Set<T>(string name, T value);

        public abstract void Set<T>(T value);

    }
}
