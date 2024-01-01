using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

internal static class NatashaAsssemblyHelper
{
    internal static Assembly[] GetRuntimeAssemblies()
    {
        var defaultContext = DependencyContext.Default;
        if (defaultContext != null)
        {
            return new HashSet<Assembly>(defaultContext
            .RuntimeLibraries
            .SelectMany(lib => lib
                .GetDefaultAssemblyNames(defaultContext)
                .Select(Assembly.Load)))
            .ToArray();
        }
        else
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var referenceNames = assembly.GetReferencedAssemblies();
                foreach (var asmName in referenceNames)
                {
                    Assembly.Load(asmName);
                }
            }
            return AppDomain.CurrentDomain.GetAssemblies();
        }
         
    }
    internal static IEnumerable<string>? GetReferenceAssmeblyFiles(Func<AssemblyName, string?, bool> excludeReferencesFunc)
    {

        IEnumerable<string>? paths = null;
        try
        {
            paths = DependencyContext.Default?
            .CompileLibraries
            .SelectMany(cl => cl.ResolveReferencePaths())
            .ToList();
        }
        catch
        {

        }

        if (paths == null || !paths.Any())
        {
            var refsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "refs");
            if (Directory.Exists(refsFolder))
            {
                paths = Directory.GetFiles(refsFolder);
            }

            refsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ref");
            if (Directory.Exists(refsFolder))
            {
                if (paths != null)
                {

                    var tempPaths = Directory.GetFiles(refsFolder);
                    if (tempPaths != null && tempPaths!.Length > 0)
                    {
                        paths = paths.Concat(tempPaths);
                    }

                }
                else
                {

                    paths = Directory.GetFiles(refsFolder);

                }
            }
        }
        return paths;

    }
}

