using Natasha.Framework;
using System;

namespace Natasha.CSharpEngine
{

    public static class SyntaxManagement
    {
        public static Func<SyntaxBase> GetSyntax;
        public static void RegisterDefault<T>() where T : SyntaxBase, new()
        {
            GetSyntax = () => { return new T();  };
        }

    }
}
