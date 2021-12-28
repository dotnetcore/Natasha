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

    internal static class CS8019Analaysistor
    {

        internal static IEnumerable<UsingDirectiveSyntax> Handler(CompilationUnitSyntax root, HashSet<Location> locations)
        {

            return from usingDeclaration in root.Usings
                        where locations.Contains(usingDeclaration.GetLocation())
                        select usingDeclaration;

        }

    }

}
