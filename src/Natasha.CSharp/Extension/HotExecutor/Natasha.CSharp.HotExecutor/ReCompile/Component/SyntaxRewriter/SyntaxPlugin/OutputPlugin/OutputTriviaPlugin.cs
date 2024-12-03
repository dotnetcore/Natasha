using Natasha.CSharp.HotExecutor.Utils;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxPlugin
{
    internal class OutputTriviaPlugin : TriviaSyntaxPluginBase
    {
        private string _debugOutputComment;
        private string _releaseOutputComment;
        private readonly Func<bool> _isRelease;
        public readonly string CommentPrefix;
        public OutputTriviaPlugin(string debugOutputComment, string releaseOutputComment, Func<bool> isRelease)
        {
            _debugOutputComment = debugOutputComment.ToLower();
            _releaseOutputComment = releaseOutputComment.ToLower();
            _isRelease = isRelease;
            CommentPrefix = "//NMSAzulx Natasha HE RID: " + Guid.NewGuid().ToString("N") + " ";
        }

        public void SetDebugOutputCommentTag(string debugOutputComment)
        {
            _debugOutputComment = debugOutputComment.ToLower();
        }

        public void SetReleaseOutputCommentTag(string releaseOutputComment)
        {
            _releaseOutputComment = releaseOutputComment.ToLower();
        }

        public override string? Handle(string comment, string lowerComment)
        {
            if (_isRelease())
            {
                return CommentPrefix + CreatePreprocessorReplaceScript(HECommentHelper.GetCommentScript(comment,_releaseOutputComment.Length));
            }
            return CommentPrefix + CreatePreprocessorReplaceScript(HECommentHelper.GetCommentScript(comment,_debugOutputComment.Length));
        }

        public override void Initialize()
        {
            
        }

        public override bool IsMatch(string comment, string lowerComment)
        {
            if (_isRelease() && lowerComment.StartsWith(_releaseOutputComment))
            {
                return true;
            }
            else if (!_isRelease() && lowerComment.StartsWith(_debugOutputComment))
            {
                return true;
            }
            return false;
        }

        private static string CreatePreprocessorReplaceScript(string content)
        {
            //if (_projectKind == HEProjectKind.AspnetCore || _projectKind == HEProjectKind.Console)
            //{
            //    return $"Console.WriteLine({content});";
            //}
            return $"HEProxy.ShowMessage(({content}).ToString());";
        }
    }
}
