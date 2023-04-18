using System;

public sealed partial class AssemblyCSharpBuilder
{

    public void ClearCompilationCache()
    {
        _compilation = null;
        SyntaxTrees.Clear();
    }
}

