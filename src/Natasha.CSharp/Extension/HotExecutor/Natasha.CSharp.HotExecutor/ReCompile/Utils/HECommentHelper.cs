namespace Natasha.CSharp.HotExecutor.Utils
{
    public static class HECommentHelper
    {
        public static string GetCommentScript(string comment, int startIndex)
        {
            if (comment.EndsWith(";"))
            {
                return comment.Substring(startIndex, comment.Length - startIndex - 1);
            }
            return comment[startIndex..];
        }
    }
}
