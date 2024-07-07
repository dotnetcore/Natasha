namespace Natasha.CSharp.HotExecutor.Component
{
    internal class CS0104TriviaPlugin : TriviaSyntaxPluginBase
    {
        public string _excludeUsingComment;
        public HashSet<string> ExcludeUsings;
        public CS0104TriviaPlugin(string excludeUsingComment)
        {
            ExcludeUsings = [];
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
            var result = CommentHelper.GetCommentScript(comment, _excludeUsingComment.Length);
            var usingStrings = result.Split(';', StringSplitOptions.RemoveEmptyEntries);
            ExcludeUsings.UnionWith(usingStrings);
            return null;
        }
    }
}
