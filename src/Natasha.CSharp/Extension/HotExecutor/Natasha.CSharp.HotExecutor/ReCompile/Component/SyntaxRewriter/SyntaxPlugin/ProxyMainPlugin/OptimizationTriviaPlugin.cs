namespace Natasha.CSharp.HotExecutor.Component.SyntaxPlugin
{
    internal class OptimizationTriviaPlugin : TriviaSyntaxPluginBase
    {
        private string _proxyCommentOPLDebug;
        private string _proxyCommentOPLRelease;
        public bool IsRelease;
        public OptimizationTriviaPlugin(string debugComment, string releaseComment)
        {
            _proxyCommentOPLDebug = debugComment.ToLower();
            _proxyCommentOPLRelease = releaseComment.ToLower();
        }

        public void SetDebugCommentTag(string debugComment)
        {
            _proxyCommentOPLDebug = debugComment.ToLower();
        }

        public void SetReleaseCommentTag(string releaseComment)
        {
            _proxyCommentOPLRelease = releaseComment.ToLower();
        }


        public override string? Handle(string comment, string lowerComment)
        {
            return null;
        }

        public override void Initialize()
        {
            IsRelease = false;
        }

        public override bool IsMatch(string comment, string lowerComment)
        {
            if (lowerComment.StartsWith(_proxyCommentOPLDebug))
            {
                IsRelease = false;
            }
            else if (lowerComment.StartsWith(_proxyCommentOPLRelease))
            {
                IsRelease = true;
            }
            return false;
        }
    }
}
