using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxPlugin
{
    public abstract class MethodSyntaxPluginBase
    {
        public readonly string MethodName;
        public readonly List<TriviaSyntaxPluginBase> TriviaSyntaxPluginBases;
        public MethodSyntaxPluginBase(string methodName)
        {
            MethodName = methodName;
            TriviaSyntaxPluginBases = [];
        }
        public abstract void Initialize();

        public MethodSyntaxPluginBase RegisteTriviaPlugin(TriviaSyntaxPluginBase triviaPlugin)
        {
            TriviaSyntaxPluginBases.Add(triviaPlugin);
            return this;
        }
        public void ClearTriviaPlugin()
        {
            TriviaSyntaxPluginBases.Clear();
        }
        public abstract BlockSyntax? Handle(BlockSyntax block);
    }
}
