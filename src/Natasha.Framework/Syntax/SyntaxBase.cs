using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Natasha.Framework
{

    public abstract class SyntaxBase
    {

        public readonly ConcurrentDictionary<string, SyntaxTree> TreeCache;
        public readonly ConcurrentDictionary<string, HashSet<string>> ReferenceCache;

        public SyntaxBase()
        {

            TreeCache = new ConcurrentDictionary<string, SyntaxTree>();
            ReferenceCache = new ConcurrentDictionary<string, HashSet<string>>();

        }


        public abstract SyntaxTree LoadTreeFromScript(string script);
        public abstract SyntaxTree LoadTree(SyntaxTree tree);




        public virtual SyntaxTree AddTreeToCache(string script)
        {
            
            if (script!=default && script != "")
            {

                SyntaxTree tree = LoadTreeFromScript(script);
                var key = tree.ToString();
                if (!TreeCache.ContainsKey(key))
                {
                    TreeCache[key] = tree;
                }
                return tree;

            }
            return null;

        }




        public virtual SyntaxTree AddTreeToCache(SyntaxTree node)
        {

            return AddTreeToCache(node.ToString());

        }


        public abstract void Update(string old, string @new, HashSet<string> sets = default);

    }

}
