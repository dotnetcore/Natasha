using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Natasha.CSharp.SemanticAnalaysis.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Natasha.CSharp.Engine.SemanticAnalaysis
{

    public static class CS8019Analaysistor
    {

        public static IEnumerable<UsingDirectiveSyntax> Handler(Diagnostic diagnostic)
        {

            var root = diagnostic.Location.SourceTree.GetRoot();
            return from usingDeclaration in root.DescendantNodes()
                          .OfType<UsingDirectiveSyntax>()
                        where usingDeclaration.GetLocation() == diagnostic.Location
                        select usingDeclaration;

        }

    }

}
