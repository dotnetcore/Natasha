// See https://aka.ms/new-console-template for more information
using DebugTest;

NatashaInitializer.Preheating();
var type = NClass
    .RandomDomain(item => item.UseNatashaFileOut().DisableSemanticCheck().ConfigCompilerOption(opt=>opt.CompileAsDebug()))
    .Public()
    .Name("a")
    .InheritanceAppend<IStandard>()
    .Method(item => item
        .Public()
        .Name("Show")
        .Body("Console.WriteLine(\"Hello World!\");"))
    .GetType();

IStandard standard = (IStandard)Activator.CreateInstance(type)!;
standard.Show();
Console.ReadKey();

static async Task Show()
{
    while (true)
    {
        //await Show2().ConfigureAwait(false);
        await Task.Delay(1000).ConfigureAwait(false);
        Console.WriteLine($"Show-1row: {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(2000).ConfigureAwait(false);
        Console.WriteLine($"Show-2row: {Thread.CurrentThread.ManagedThreadId}");
    }
}

static async Task Show2()
{
    await Task.Delay(1000).ConfigureAwait(false);
    Console.WriteLine($"Show2: {Thread.CurrentThread.ManagedThreadId}");
}
