using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxPlugin
{
    internal class ProxyMethodPlugin : MethodSyntaxPluginBase
    {
        private HEProjectKind _kind;
        public string ClassName = string.Empty;
        public ProxyMethodPlugin(string methodName) : base(methodName)
        {
            _kind = HEProjectKind.Console;
        }
        public void SetProjectKind(HEProjectKind kind)
        {
            _kind = kind;
        }
        public override BlockSyntax? Handle(BlockSyntax blockSyntax)
        {
            ClassDeclarationSyntax? parentClass = blockSyntax.Parent!.Parent as ClassDeclarationSyntax ?? throw new Exception($"获取 {MethodName} 方法类名出现错误！");
            ClassName = parentClass.Identifier.Text;

            if (_kind == HEProjectKind.Winform)
            {
                return WinformRewriter.Handle(blockSyntax);
            }
            else if (_kind == HEProjectKind.WPF)
            {
                return WpfWriter.Handle(blockSyntax);
            }
            else if (_kind == HEProjectKind.Console)
            {
                return ConsoleWriter.Handle(blockSyntax);
            }
            return null;
        }

        public override void Initialize()
        {
            
        }
    }
}
