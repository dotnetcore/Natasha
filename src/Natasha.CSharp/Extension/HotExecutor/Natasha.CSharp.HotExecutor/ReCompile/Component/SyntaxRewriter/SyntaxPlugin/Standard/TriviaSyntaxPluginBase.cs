namespace Natasha.CSharp.HotExecutor.Component.SyntaxPlugin
{
    public abstract class TriviaSyntaxPluginBase
    {
        public abstract void Initialize();
        public abstract bool IsMatch(string comment, string lowerComment);

        public abstract string? Handle(string comment, string lowerComment);
    }
}
