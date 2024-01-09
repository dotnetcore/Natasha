namespace Natasha.CSharp.Codecov.Utils
{
    internal static class MethodDefinedFilter
    {
        private static readonly HashSet<string> _defined;
        private static readonly HashSet<string> _startWith;
        static MethodDefinedFilter()
        {
            _defined = ["Runtime.Instrumentation.CreatePayload", "Runtime.Instrumentation.FlushPayload", "<PrivateImplementationDetails>..cctor"];
            _startWith = ["CompilerServices.", "CodeAnalysis.EmbeddedAttribute."];
        }

        public static bool IsMatch(string methodDefinedName)
        {
            if (!_defined.Contains(methodDefinedName))
            {
                foreach (var item in _startWith)
                {
                    if (methodDefinedName.StartsWith(item))
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }
    }
}
