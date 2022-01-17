// See https://aka.ms/new-console-template for more information

using PluginBase;
using System.Reflection;

//refelct type, assembly will be loaded in default context.
_ = typeof(IPluginBase);
_ = typeof(Dapper.CommandDefinition);

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Base: PluginBase");
Console.WriteLine("\t PluginA -> Dapper2.0 & PluginBase");
Console.WriteLine("\t PluginB -> Dapper1.6 & PluginBase");
Console.WriteLine("\t Program -> Dapper1.6 & PluginBase");
Console.ResetColor();


string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("\\PluginSample\\bin\\Debug\\net6.0", ""), "{0}\\bin\\Debug\\net6.0\\{0}.dll");
Console.WriteLine("\r\nLoad all version without excluding base:");
ShowAllVersion(path,false);

Console.WriteLine("\r\nLoad all version with excluding base:");
ShowAllVersion(path,true);

Console.WriteLine("\r\nUse Default Version:");
ShowDefaultVersion(path);

Console.WriteLine("\r\nUse Low Version:");
ShowLowVersion(path);

Console.WriteLine("\r\nUse High Version:");
ShowHighVersion(path);

Console.WriteLine("\r\nUse Dapper Major Version = 2:");
ShowSpecialVersion(path, 2);

Console.ReadKey();

static void ShowAllVersion(string path, bool excludeBase)
{
    NatashaDomain domain = new(Guid.NewGuid().ToString());
    ExcutindAndShowException(() =>
    {

        var assemblyA = domain.LoadPluginWithAllDependency(string.Format(path, "PluginA"), excludeBase ? (name) => name.Name!.Contains("PluginBase") : null);
        ShowVersion(assemblyA);

    });
    ExcutindAndShowException(() =>
    {

        var assemblyB = domain.LoadPluginWithAllDependency(string.Format(path, "PluginB"), excludeBase ? (name) => name.Name!.Contains("PluginBase") : null);
        ShowVersion(assemblyB);

    });
}

static void ShowDefaultVersion(string path)
{
    NatashaDomain domain = new(Guid.NewGuid().ToString());
    ExcutindAndShowException(() =>
    {

        var assemblyA = domain.LoadPluginUseDefaultDependency(string.Format(path, "PluginA"));
        ShowVersion(assemblyA);

    });
    ExcutindAndShowException(() =>
    {

        var assemblyB = domain.LoadPluginUseDefaultDependency(string.Format(path, "PluginB"));
        ShowVersion(assemblyB);

    });
}


static void ShowLowVersion(string path)
{
    NatashaDomain domain = new(Guid.NewGuid().ToString());
    ExcutindAndShowException(() =>
    {

        var assemblyA = domain.LoadPluginWithLowDependency(string.Format(path, "PluginA"));
        ShowVersion(assemblyA);

    });
    ExcutindAndShowException(() =>
    {

        var assemblyB = domain.LoadPluginWithLowDependency(string.Format(path, "PluginB"));
        ShowVersion(assemblyB);

    });
}

static void ShowHighVersion(string path)
{
    NatashaDomain domain = new(Guid.NewGuid().ToString());
    ExcutindAndShowException(() =>
    {

        var assemblyA = domain.LoadPluginWithHighDependency(string.Format(path, "PluginA"));
        ShowVersion(assemblyA);

    });
    ExcutindAndShowException(() =>
    {

        var assemblyB = domain.LoadPluginWithHighDependency(string.Format(path, "PluginB"));
        ShowVersion(assemblyB);

    });
}


static void ShowSpecialVersion(string path, int majorVersion)
{
    NatashaDomain domain = new(Guid.NewGuid().ToString());
    ExcutindAndShowException(() =>
    {

        var assemblyA = domain.LoadPluginWithAllDependency(string.Format(path, "PluginA"), asmName => {
            if (asmName.Name!.Contains("PluginBase"))
            {
                return true;
            }
            if (asmName.Name!.Contains("Dapper") && asmName.Version!.Major != majorVersion)
            {
                return true;
            }
            return false;
        });
        ShowVersion(assemblyA);

    });
    ExcutindAndShowException(() =>
    {

        var assemblyB = domain.LoadPluginWithAllDependency(string.Format(path, "PluginB"), asmName => {
            if (asmName.Name!.Contains("PluginBase"))
            {
                return true;
            }
            if (asmName.Name!.Contains("Dapper") && asmName.Version!.Major != majorVersion)
            {
                return true;
            }
            return false;
        });
        ShowVersion(assemblyB);

    });
}


static void ShowVersion(Assembly assembly)
{
    var asmName = assembly.GetName().Name!;
    var type = assembly.GetTypes().Where(item => asmName.Contains(item.Name)).First();
    var plugin = (IPluginBase)(Activator.CreateInstance(type)!);
    plugin.ShowVersion();
}


static void ExcutindAndShowException(Action acion)
{
    try
    {
        Console.ForegroundColor = ConsoleColor.Green;
        acion();
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"报错:{ex.Message}");
        Console.ResetColor();
    }
}