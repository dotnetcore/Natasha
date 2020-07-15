using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Natasha.Framework
{

    public abstract class SyntaxBase
    {

        /// <summary>
        /// 语法树缓存
        /// </summary>
        public readonly ConcurrentDictionary<string, SyntaxTree> TreeCache;
        /// <summary>
        /// 引用缓存
        /// </summary>
        public readonly ConcurrentDictionary<string, HashSet<string>> ReferenceCache;

        public SyntaxBase()
        {

            TreeCache = new ConcurrentDictionary<string, SyntaxTree>();
            ReferenceCache = new ConcurrentDictionary<string, HashSet<string>>();

        }


        /// <summary>
        /// 从脚本中加载语法树，需要重载
        /// </summary>
        /// <param name="script">脚本代码</param>
        /// <returns></returns>
        public abstract SyntaxTree LoadTreeFromScript(string script);
        /// <summary>
        /// 直接加载语法树，需要重载
        /// </summary>
        /// <param name="tree">语法树</param>
        /// <returns></returns>
        public abstract SyntaxTree LoadTree(SyntaxTree tree);



        /// <summary>
        /// 从脚本中添加语法树，并缓存
        /// 该方法对外暴露，但需要重载 LoadTreeFromScrip 来实现内部功能
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
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



        /// <summary>
        /// 加载语法树并缓存
        /// 该方法对外暴露，但需要重载 LoadTree 来实现内部功能
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual SyntaxTree AddTreeToCache(SyntaxTree node)
        {

            return AddTreeToCache(node.ToString());

        }


        /// <summary>
        /// 更新语法树
        /// </summary>
        /// <param name="oldCode">旧代码</param>
        /// <param name="newCode">新代码</param>
        /// <param name="sets">新的引用</param>
        public abstract void Update(string oldCode, string newCode, HashSet<string> sets = default);

    }

}
