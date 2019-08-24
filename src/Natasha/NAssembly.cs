using Natasha.Operator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha
{
    public class NAssembly
    {

        public Assembly Assembly;
        private readonly HashSet<IScript> _builderCache;
        public readonly AssemblyComplier Options;
        public readonly ConcurrentDictionary<string, Type> TypeCache;


        public NAssembly(string name) : this()
        {

            Options.Name = name;

        }


        public NAssembly()
        {

            _builderCache = new HashSet<IScript>();
            TypeCache = new ConcurrentDictionary<string, Type>();
            Options = new AssemblyComplier();

        }




        public bool Remove(IScript builder)
        {
            return _builderCache.Remove(builder);
        }




        public OopOperator CreateClass(string name = default)
        {
            var @operator = new OopOperator().OopName(name).Namespace(Options.Name).ChangeToClass();
            _builderCache.Add(@operator);
            return @operator;

        }




        public OopOperator CreateEnum(string name = default)
        {

            var @operator = new OopOperator().OopName(name).Namespace(Options.Name).ChangeToEnum();
            _builderCache.Add(@operator);
            return @operator;

        }




        public OopOperator CreateInterface(string name = default)
        {

            var @operator = new OopOperator().OopName(name).Namespace(Options.Name).ChangeToInterface();
            _builderCache.Add(@operator);
            return @operator;

        }




        public OopOperator CreateStruct(string name = default)
        {

            var @operator = new OopOperator().OopName(name).Namespace(Options.Name).ChangeToStruct();
            _builderCache.Add(@operator);
            return @operator;
        }




        public FastMethodOperator CreateFastMethod(string name = default)
        {

            var @operator = new FastMethodOperator().OopName(name);
            _builderCache.Add(@operator);
            return @operator;

        }




        public FakeMethodOperator CreateFakeMethod(string name = default)
        {

            var @operator = new FakeMethodOperator().OopName(name);
            _builderCache.Add(@operator);
            return @operator;

        }




        public List<CompilationException> Check()
        {

            foreach (var item in _builderCache)
            {
                Options.Add(item);
            }
            return Options.ComplierInfos.Exceptions;

        }




        public Assembly Complier()
        {

            Check();
            Assembly = Options.GetAssembly();
            var types = Assembly.GetTypes();
            foreach (var item in types)
            {
                TypeCache[item.GetDevelopName()] = item;
            }
            return Assembly;

        }



        public Type GetType(string name)
        {

            if (TypeCache.ContainsKey(name))
            {
                return TypeCache[name];
            }
            return default;

        }

    }

}
