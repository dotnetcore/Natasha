using Microsoft.CodeAnalysis;
using Natasha.CSharpEngine.Error;
using Natasha.CSharpEngine.Log;
using Natasha.CSharpSyntax;
using Natasha.Error;
using Natasha.Error.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace Natasha.CSharpEngine.Syntax
{

    public class NatashaCSharpSyntax : CSharpSyntaxBase
    {

        public readonly ConcurrentDictionary<string, HashSet<string>> UsingCache;
        
        public NatashaCSharpSyntax()
        {
            UsingCache = new ConcurrentDictionary<string, HashSet<string>>();
        }




        public virtual void Update(string old, string @new, HashSet<string> sets = default)
        {

            if (TreeCache.ContainsKey(old))
            {

                while (!TreeCache.TryRemove(old, out _)) { };

            }
            if (sets == default)
            {

                if (UsingCache.ContainsKey(old))
                {
                    sets = UsingCache[old];
                    while (!UsingCache.TryRemove(old, out _)) { };
                }

            }

            AddTreeToCache(@new);
            UsingCache[@new] = sets;

        }

    }
}
