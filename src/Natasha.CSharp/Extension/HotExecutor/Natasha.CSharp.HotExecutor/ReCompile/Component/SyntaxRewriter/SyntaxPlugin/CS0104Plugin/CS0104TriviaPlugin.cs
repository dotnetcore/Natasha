namespace Natasha.CSharp.HotExecutor.Component.SyntaxPlugin
{
    using Natasha.CSharp.HotExecutor.Utils;
    using System.Collections.Generic;
    internal class CS0104TriviaPlugin : TriviaSyntaxPluginBase
    {
        public string _excludeUsingComment;
        public HashSet<string> ExcludeUsings;
        public CS0104TriviaPlugin(string excludeUsingComment)
        {
            ExcludeUsings = new HashSet<string>();
            _excludeUsingComment = excludeUsingComment.ToLower();
        }
        public void SetMatchComment(string excludeUsingComment)
        {
            _excludeUsingComment = excludeUsingComment.ToLower();
        }
        public override void Initialize()
        {
            ExcludeUsings.Clear();
        }
        public override bool IsMatch(string comment, string lowerComment)
        {
            return lowerComment.StartsWith(_excludeUsingComment);
        }
        public override string? Handle(string comment, string lowerComment)
        {
            var result = HECommentHelper.GetCommentScript(comment, _excludeUsingComment.Length);
            var usingStrings = result.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
            ExcludeUsings.UnionWith(usingStrings);
            return null;
        }
    }
}
