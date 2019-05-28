using Natasha.Complier;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Natasha
{
    public abstract class IComplier : IScriptBuilder
    {

        public virtual void SingleError(string msg)
        {
#if DEBUG
            Debug.WriteLine(msg);
#endif
        }

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

        public Assembly GetAssemblyByScript(string name=null)
        {
            if (!_useFileComplie)
            {
                return ScriptComplier.StreamComplier(Builder(), name, SingleError);
            }
            else
            {
                return ScriptComplier.FileComplier(Builder(), name, SingleError);
            }
        }

        public virtual Delegate Complie()
        {
            return null;
        }

        public virtual string Builder()
        {
            return "";
        }
    }
}
