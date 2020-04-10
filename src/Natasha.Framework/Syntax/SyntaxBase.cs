using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;

namespace Natasha.Framework
{

    public abstract class SyntaxBase
    {

        public readonly ConcurrentDictionary<string, SyntaxTree> TreeCache;
        

        public SyntaxBase()
        {

            TreeCache = new ConcurrentDictionary<string, SyntaxTree>();

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

    }

}
