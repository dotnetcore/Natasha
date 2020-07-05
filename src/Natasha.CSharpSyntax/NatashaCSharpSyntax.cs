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
    public override SyntaxTree LoadTree(SyntaxTree tree)
    {

        using (var workspace = new AdhocWorkspace())
        {
            SyntaxNode root = Formatter.Format(tree.GetRoot(), workspace);
            tree = root.SyntaxTree;
        }
        return tree;

    }


    public override void Update(string old, string @new, HashSet<string> sets = default)
    {

        if (TreeCache.ContainsKey(old))
        {

            while (!TreeCache.TryRemove(old, out _)) { };

        }
        if (sets == default)
        {

            if (ReferenceCache.ContainsKey(old))
            {
                sets = ReferenceCache[old];
                while (!ReferenceCache.TryRemove(old, out _)) { };
            }

        }

        AddTreeToCache(@new);
        ReferenceCache[@new] = sets;

    }

}

