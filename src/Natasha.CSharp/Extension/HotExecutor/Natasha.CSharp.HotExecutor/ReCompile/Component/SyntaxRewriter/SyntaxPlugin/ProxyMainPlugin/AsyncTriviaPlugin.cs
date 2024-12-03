namespace Natasha.CSharp.HotExecutor.Component.SyntaxPlugin
{
    internal class AsyncTriviaPlugin : TriviaSyntaxPluginBase
    {
        private string _asyncCommentTag;
        public bool IsAsync;
        public AsyncTriviaPlugin(string asyncComment)
        {
            _asyncCommentTag = asyncComment.ToLower();
        }
        public void SetAsyncCommentTag(string asyncComment)
        {
            _asyncCommentTag = asyncComment.ToLower();
        }
        public override string? Handle(string comment, string lowerComment)
        {
            IsAsync = true;
            return null;
        }

        public override void Initialize()
        {
            IsAsync = false;
        }

        public override bool IsMatch(string comment, string lowerComment)
        {
            return lowerComment.StartsWith(_asyncCommentTag);
        }
    }
}
