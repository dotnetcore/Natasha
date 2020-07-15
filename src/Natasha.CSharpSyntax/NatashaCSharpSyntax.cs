using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Natasha.Framework;
using System.Collections.Generic;


public class NatashaCSharpSyntax : SyntaxBase
{

    //private readonly static AdhocWorkspace _workSpace;
    private readonly static CSharpParseOptions _options;

    static NatashaCSharpSyntax()
    {

        //_workSpace = new AdhocWorkspace();
        //_workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));
        _options = new CSharpParseOptions(LanguageVersion.Latest, preprocessorSymbols: new[] { "RELEASE" });

    }


    public override SyntaxTree LoadTreeFromScript(string script)
    {

        var tree = SyntaxFactory.ParseSyntaxTree(script.Trim(), _options); ;
        return LoadTree(tree);

    }


    /// <summary>
    /// 直接加载树，并缓存
    /// </summary>
    /// <param name="tree"></param>
    /// <returns></returns>
    public override SyntaxTree LoadTree(SyntaxTree tree)
    {

        using (var workspace = new AdhocWorkspace())
        {
            SyntaxNode root = Formatter.Format(tree.GetRoot(), workspace);
            tree = root.SyntaxTree;
        }
        return tree;

    }


    /// <summary>
    /// 更新语法树
    /// </summary>
    /// <param name="oldCode">旧代码</param>
    /// <param name="newCode">新代码</param>
    /// <param name="sets">新的引用</param>
    public override void Update(string oldCode, string newCode, HashSet<string> sets = default)
    {

        //先移除
        if (TreeCache.ContainsKey(oldCode))
        {

            while (!TreeCache.TryRemove(oldCode, out _)) { };

        }
        if (sets == default)
        {

            if (ReferenceCache.ContainsKey(oldCode))
            {
                sets = ReferenceCache[oldCode];
                while (!ReferenceCache.TryRemove(oldCode, out _)) { };
            }

        }

        //再添加
        AddTreeToCache(newCode);
        ReferenceCache[newCode] = sets;

    }

}

