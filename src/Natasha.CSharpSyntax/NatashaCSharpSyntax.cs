using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Text;
using Natasha.Framework;
using System;
using System.Collections.Generic;


public class NatashaCSharpSyntax : SyntaxBase
{

    //private readonly static AdhocWorkspace _workSpace;
    private readonly static CSharpParseOptions _options;
    //private readonly static OptionSet _formartOptions;
    static NatashaCSharpSyntax()
    {

        _options = new CSharpParseOptions(LanguageVersion.Latest);
        //var workspace = new AdhocWorkspace();
        //_formartOptions = workspace.Options;
        ////_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
        ////_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.IndentBlock, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.IndentSwitchCaseSection, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.IndentSwitchCaseSectionWhenBlock, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.IndentSwitchSection, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.LabelPositioning, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLineForCatch, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLineForClausesInQuery, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLineForElse, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLineForFinally, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLineForMembersInAnonymousTypes, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLineForMembersInObjectInit, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAccessors, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInControlBlocks, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceAfterCast, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceAfterColonInBaseTypeDeclaration, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceAfterComma, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceAfterControlFlowStatementKeyword, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceAfterDot, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceAfterMethodCallName, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceAfterSemicolonsInForStatement, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceBeforeColonInBaseTypeDeclaration, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceBeforeComma, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceBeforeDot, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceBeforeOpenSquareBracket, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceBeforeSemicolonsInForStatement, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptyMethodCallParentheses, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptyMethodDeclarationParentheses, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptySquareBrackets, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpacesIgnoreAroundVariableDeclaration, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceWithinCastParentheses, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceWithinExpressionParentheses, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceWithinMethodCallParentheses, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceWithinMethodDeclarationParenthesis, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceWithinOtherParentheses, false);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpaceWithinSquareBrackets, false);
        ////_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpacingAfterMethodDeclarationName, true);
        ////_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.SpacingAroundBinaryOperator, _options.Language);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, true);
        //_formartOptions = _formartOptions.WithChangedOption(CSharpFormattingOptions.WrappingPreserveSingleLine, true);

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
            tree = Formatter.Format(tree.GetRoot(), workspace).SyntaxTree;
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

