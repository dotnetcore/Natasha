using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Utils
{
    public class NamespaceGetter
    {
        public static string GetNamespace(string type)
        {
            return NamespaceFunc(type)();
        }

        public static Func<string> NamespaceFunc(string type){
            ScriptBuilder builder = new ScriptBuilder();
            return builder
                .Body($" return typeof({type}).Namespace;")
                .Return<string>()
                .Create<Func<string>>();
        }

    }
}
