using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Natasha.CSharp.Template
{
    public class FlagTemplate<T> : NamespaceTemplate<T> where T : FlagTemplate<T>, new()
    {
        public HashSet<Assembly> AssemblyCache;
        public StringBuilder FlagScript;
        public FlagTemplate()
        {
            FlagScript = new StringBuilder();
            AssemblyCache = new HashSet<Assembly>();
        }


        public T SetFlag(string script)
        {
            FlagScript.AppendLine($"[{script}]");
            return Link;
        }

        public T SetFlag(Type attrType,string script, string flag = default)
        {
            SetFlag($"{flag}:{attrType.GetDevelopName()}(\"{script}\")");
            return Link;
        }

        public T SetFlag<S>(string script, string flag = default)
        {
            return SetFlag(typeof(S),script,flag);
        }

        public T SetAssemblyFlag<S>(string script)
        {
            return SetFlag(typeof(S), script, "assembly");
        }

        public T SetAssemblyFlag(Type attrType, string script)
        {
            return SetFlag(attrType, script, "assembly");
        }

        public T SetAssemblyFlag(string script)
        {
            return SetFlag("assembly:"+script);
        }
        public T AllowPrivate(string assemblyName)
        {
            SetAssemblyFlag<IgnoresAccessChecksToAttribute>(assemblyName);
            return Link;
        }
        public T AllowPrivate(Assembly assembly)
        {
            if (!AssemblyCache.Contains(assembly))
            {
                AssemblyCache.Add(assembly);
                return AllowPrivate(assembly.GetName().Name);
            }
            return Link;
        }
        public T AllowPrivate<S>()
        {
            return AllowPrivate(typeof(S));
        }
        public T AllowPrivate(Type type)
        {
            return AllowPrivate(type.Assembly);
        }
        public T AllowPrivate(MemberInfo info)
        {
            return AllowPrivate(info.DeclaringType);
        }



        public override T BuilderScript()
        {

            //  [{this}]
            //  [Namspace]
            //  [Namspace]
            //  [Attribute]
            //  [access] [modifier] [Name] [:Interface] 
            //  [body]
            base.BuilderScript();
            _script.Insert(0, FlagScript);
            return Link;

        }
    }
}
