using System;

namespace Natasha
{
    public abstract class IComplier
    {
        protected bool _useFileComplie;
        public IComplier UseFileComplie(bool shut = true)
        {
            _useFileComplie = shut;
            return this;
        }

        public Delegate Create()
        {
            return Complie();
        }

        public T Create<T>() where T: Delegate
        {
            return (T)Complie();
        }

        public virtual Delegate Complie()
        {
            return null;
        }
    }
}
