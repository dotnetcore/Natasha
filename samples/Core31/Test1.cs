using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Core31
{
    public class Test1
    {
        public Test1(int a)
        {

        }
    }

    public static class OopExtesnion
    {
        public static Equit<T> WithScript<T>(this T value,string script)
        {
            return new Equit<T>(value, script);
        }
    }


    public class Equit<T>
    {
        private readonly T _value;
        private static ConcurrentDictionary<string, Delegate> _currentCache;
        private readonly string _script;
        static Equit()
        {
            _currentCache = new ConcurrentDictionary<string, Delegate>();
        }
        public Equit(T value, string script)
        {
            _value = value;
            _script = script;
        }

        public S Execute<S>()
        {
            if (!_currentCache.TryGetValue(_script,out var @delegate))
            {
                var action = NDelegate
                    .RandomDomain()
                    //.WithFirstArgInvisible()
                    .Func<T, S>(_script);
                _currentCache[_script] = @delegate;
                return action(_value);
            }
            return ((Func<T, S>)@delegate)(_value);
        }
        public void Execute()
        {
            if (!_currentCache.TryGetValue(_script, out var @delegate))
            {
                var action = NDelegate
                    .RandomDomain()
                    //.WithFirstArgInvisible()
                    .Action<T>(_script);
                _currentCache[_script] = @delegate;
                 action(_value);
                return;
            }
            ((Action<T>)@delegate)(_value);
        }

    }
}
