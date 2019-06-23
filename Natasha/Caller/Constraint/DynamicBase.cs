using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public abstract class DynamicBase<T> : DynamicBase {
        public T Instance;
        public void SetInstance(T value) {
            Instance = value;
        }
    }
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
        public virtual void New() { }

        public abstract DynamicBase Get(string name);

        public abstract T Get<T>(string name);

        public abstract T Get<T>();

        public abstract void Set<T>(string name, T value);

        public abstract void Set<T>(T value);

    }
}
