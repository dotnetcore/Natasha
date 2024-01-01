using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Runtime.CompilerServices;

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

        public T SetFlag(Type attrType, string script, string flag = "")
        {
            SetFlag($"{flag}:{attrType.GetDevelopName()}(\"{script}\")");
            return Link;
        }

        public T SetFlag<S>(string script, string flag = "")
        {
            return SetFlag(typeof(S), script, flag);
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
            return SetFlag("assembly:" + script);
        }

        public T AllowPrivate(string? assemblyName)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                SetAssemblyFlag<IgnoresAccessChecksToAttribute>(assemblyName);
            }
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
        public T AllowPrivate(Type? type)
        {
            if (type != null)
            {
                return AllowPrivate(type.Assembly);
            }
            return Link;
        }
        public T AllowPrivate(MemberInfo info)
        {
            return AllowPrivate(info.DeclaringType);
        }

        public override T BuilderScript()
        {

            //  [{this}]
            //  [namspace]
            //  { 
            //      [comment]
            //      [attribute]
            //      [access] [modifier] [Name] [:Interface] 
            //      {
            //          [body]
            //      }
            //      [OtherBody]
            //  }
            base.BuilderScript();
            _script.Insert(0, FlagScript);
            return Link;

        }
    }
}
