using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Natasha.CSharp.Engine.SemanticAnalaysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public static class UsingAnalysistorManagement
{
    private readonly static ConcurrentDictionary<string, Func<Diagnostic, IEnumerable<UsingDirectiveSyntax>>> _errorHandlers;
    static UsingAnalysistorManagement()
    {

        _errorHandlers = new ConcurrentDictionary<string, Func<Diagnostic, IEnumerable<UsingDirectiveSyntax>>>();
        _errorHandlers["CS0234"] = CS0234Analaysistor.Handler;
        _errorHandlers["CS0246"] = CS0246Analaysistor.Handler;
        _errorHandlers["CS8019"] = CS8019Analaysistor.Handler;
    }

    public static void Add(string code, Func<Diagnostic, IEnumerable<UsingDirectiveSyntax>> func)
    {
        _errorHandlers[code] = func;
    }

    public static bool CanHandle(string errorCode)
    {
        return _errorHandlers.ContainsKey(errorCode);
    }

    public static IEnumerable<UsingDirectiveSyntax> Handle(Diagnostic diagnostic)
    {
        if (_errorHandlers.TryGetValue(diagnostic.Id,out var handler))
        {
            return handler(diagnostic);
        }
        return null;
    }

}

