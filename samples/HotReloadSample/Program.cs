using HotReloadPlugin;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CodeGen;
using Microsoft.CodeAnalysis.Collections;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection.Emit;
using System.Text;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.DependencyModel;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
namespace HotReloadSample
{
    internal class Program
    {
        static unsafe void Main(string[] args)
        {
            NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();

           // AssemblyCSharpBuilder builder = new();
           // builder.UseRandomDomain();
           // builder.UseSimpleMode();
           // builder.WithoutCombineUsingCode();
           // builder.ConfigCompilerOption(opt => opt
           // .AddSupperess("CS8618")
           //// .WithDiagnosticLevel(ReportDiagnostic.Info)
           // );
           // builder.ConfigLoadContext(opt => 
           //     opt
           //     .AddReferenceAndUsingCode<object>()
           //     .AddReferenceAndUsingCode<Task>()
           // );
           // builder.Add("using System.Threading.Tasks; public class A{ public string Name; public async Task Delay() {return;} }");
           // builder.SetLogEvent((log) =>
           // {
           //     Console.WriteLine(log.ToString());
           // });
           // var asm = builder.GetAssembly();
           // Console.WriteLine(asm.FullName);



            Console.WriteLine(MetadataUpdater.IsSupported);
            Console.WriteLine(Debugger.IsAttached);
            //Console.WriteLine(Vector128<short>.Count); 
            //var asm = typeof(A).Assembly;
            //Stopwatch stopwatch = Stopwatch.StartNew();
            //HashSet<string> names = new HashSet<string>();
            //HashSet<string> names1 = new HashSet<string>();
            //foreach (var item in asm.ExportedTypes)
            //{
            //    if (item.Namespace!=null)            //    {
            //        names.Add(item.Namespace);
            //    }
            //}
            //stopwatch.Stop();
            //Console.WriteLine("从反射中获取命名空间：" + stopwatch.Elapsed);
            //stopwatch.Restart();
            //if (asm.TryGetRawMetadata(out var blob, out var length))
            //{

            //    //var metadataReference = AssemblyMetadata.Create(ModuleMetadata.CreateFromMetadata((IntPtr)blob, length)).GetReference();
            //    var metaReader = new MetadataReader(blob, length);
            //    //var asmInfo = metaReader.GetAssemblyDefinition();
            //    var asmDefinition = metaReader.GetNamespaceDefinitionRoot();
            //    foreach (var handle in asmDefinition.NamespaceDefinitions)
            //    {
            //        //var nameDefinition = metaReader.GetNamespaceDefinition(handle);
            //        names1.Add(metaReader.GetString(handle));
            //        //Console.WriteLine($"{nameDefinition.Name.IsNil} : {metaReader.GetString(handle)} : {nameDefinition.TypeDefinitions.Count()}");
            //    }
            //}
            //stopwatch.Stop();
            //Console.WriteLine("从内存中获取命名空间：" + stopwatch.Elapsed);



            //Test(typeof(A).Assembly);
            //HotReloadService.UpdateApplicationEvent += HotReloadService_UpdateApplicationEvent;
            //NatashaManagement.Preheating(null,false,true);
            var domain = NatashaManagement.CreateRandomDomain();
            var asm1 = OldAssembly(domain);
            var typeA = asm1.GetTypeFromShortName("A");
            var methodInfo = typeA.GetMethod("Show");
            var obj = Activator.CreateInstance(typeA);
             methodInfo.Invoke(obj, null);
            Console.WriteLine();
            Assembly asm2 = asm1;
            try
            {
                asm2 = NewAssembly(domain, asm1);
            }
            catch (Exception ex)
            {
                var ex1 = ex as InvalidOperationException;
                Console.WriteLine(ex1.Message);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return;
            }
           
            var members = asm2.GetTypeFromShortName("A").GetMembers();
            var typeA1 = asm2.GetTypeFromShortName("A");
            methodInfo = typeA1.GetMethod("Show");
            var obj1 = Activator.CreateInstance(typeA1);
            methodInfo.Invoke(obj1, null);
            Console.ReadKey();
        }

        //private static void HotReloadService_UpdateApplicationEvent(Type[]? obj)
        //{
        //    Console.WriteLine(obj==null);
        //}

        public static Assembly OldAssembly(NatashaLoadContext domain)
        {
            AssemblyCSharpBuilder builder = new()
            {
                LoadContext = domain
            };
            builder.UseSimpleMode();
            builder.ConfigLoadContext(opt => opt
                .AddReferenceAndUsingCode(typeof(object))
                .AddReferenceAndUsingCode(typeof(Console))
                );
            builder.WithDebugCompile(opt => opt.WriteToAssembly());
            builder.Add("public class A{  public int Code = 1; public void Show(){ Console.WriteLine(Code);  }   }");
            return builder.GetAssembly();
        }
        //CTRL+F5
        public static Assembly NewAssembly(NatashaLoadContext domain, Assembly oldAssembly)
        {
            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder(oldAssembly.GetName().Name);
            builder.LoadContext = domain;
            builder.WithFileOutput();
            builder.UseSimpleMode();
            builder.ConfigLoadContext(opt => opt
                .AddReferenceAndUsingCode(typeof(object))
                .AddReferenceAndUsingCode(typeof(Console))
                );
            //builder.WithDebugCompile(opt => opt.WriteToAssembly());
            builder.Add("public class A{  public int Code = 2; public int Code1 = 0; public void Show(){ Console.WriteLine(Code);  }   }");
            var result = builder.UpdateAssembly(oldAssembly);
            //Console.WriteLine(1);
            //MetadataUpdater.ApplyUpdate(result.Item1, result.Item2, result.Item3, result.Item4);
            //return result.Item1;
            return result.Item1;
        }

        public static void Show(Assembly assembly)
        {
            var type = assembly.GetType("A");
            var method = type!.GetMethod("Show");
            method!.Invoke(Activator.CreateInstance(type), null);
        }
        public unsafe static void Update(Assembly oldAssembly,Assembly newAssembly)
        {
            Console.WriteLine(RuntimeFeature.IsDynamicCodeCompiled);
            Console.WriteLine(MetadataUpdater.IsSupported);
            if (newAssembly.TryGetRawMetadata(out var newBlob, out var newLength))
            {
                ReadOnlySpan<byte> newMetadataSpan = new(newBlob, newLength);
                MetadataUpdater.ApplyUpdate(oldAssembly, newMetadataSpan, Encoding.UTF8.GetBytes(File.ReadAllText("1.txt")), ReadOnlySpan<byte>.Empty);
            }

        }
    }
}
