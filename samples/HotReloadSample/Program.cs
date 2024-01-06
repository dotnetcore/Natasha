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

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
namespace HotReloadSample
{
    internal class Program
    {
        static unsafe void Main(string[] args)
        {
            Console.WriteLine(MetadataUpdater.IsSupported);
            //Console.WriteLine(Vector128<short>.Count); 
            //var asm = typeof(A).Assembly;
            //Stopwatch stopwatch = Stopwatch.StartNew();
            //HashSet<string> names = new HashSet<string>();
            //HashSet<string> names1 = new HashSet<string>();
            //foreach (var item in asm.ExportedTypes)
            //{
            //    if (item.Namespace!=null)
            //    {
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
            //var domain = NatashaManagement.CreateRandomDomain();
            ////var asm1 = OldAssembly(domain);
            ////Show(asm1);
            //var asm2 = NewAssembly(domain);
            //Show(asm2);
            //Update(typeof(A).Assembly, asm2);
            //Show(typeof(A).Assembly);
            ////MetadataUpdater.ApplyUpdate();
            Console.ReadKey();
            
        }

        public  unsafe static void Test(Assembly assembly)
        {
            if (assembly.TryGetRawMetadata(out var blob, out var length))
            {
                //HXD04
                //var metadataReference = AssemblyMetadata.Create(ModuleMetadata.CreateFromMetadata((IntPtr)blob, length)).GetReference();
                var metaReader = new MetadataReader(blob, length);
                foreach (var item in metaReader.MethodDefinitions)
                {
                    var methodDefine = metaReader.GetMethodDefinition(item);
                    var methodName = metaReader.GetString(methodDefine.Name);
                    if (methodName == "Show2")
                    {
                        Console.WriteLine(metaReader.GetString(metaReader.GetTypeDefinition(methodDefine.GetDeclaringType()).Name));
                        Console.WriteLine(Encoding.UTF8.GetString(metaReader.GetBlobBytes(methodDefine.Signature)));
                        foreach (var parameter in methodDefine.GetParameters())
                        {
                            var para = metaReader.GetParameter(parameter);
                            foreach (var paraAttr in para.GetCustomAttributes())
                            {
                                Console.WriteLine(Encoding.UTF8.GetString(metaReader.GetBlobBytes(metaReader.GetCustomAttribute(paraAttr).Value)));
                            }
                            Console.WriteLine(metaReader.GetString(para.Name));
                        }
                        Console.WriteLine();
                    }
                    
                }
               

                var asmDefinition = metaReader.GetNamespaceDefinitionRoot();
                foreach (var handle in asmDefinition.NamespaceDefinitions)
                {
                    var nameDefinition = metaReader.GetNamespaceDefinition(handle);
                    Console.WriteLine($"{nameDefinition.Name.IsNil} : {metaReader.GetString(handle)} : {nameDefinition.TypeDefinitions.Length}");
                }           
            }
        }
        private static void HotReloadService_UpdateApplicationEvent(Type[]? obj)
        {
            Console.WriteLine(obj==null);
        }

        public static Assembly OldAssembly(NatashaLoadContext domain)
        {
            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder
            {
                LoadContext = domain
            };
            builder.Add("public class A{  public int Code = 1; public void Show(){ Console.WriteLine(Code);  }   }", UsingLoadBehavior.WithDefault);
            return builder.GetAssembly();
        }

        public static Assembly NewAssembly(NatashaLoadContext domain)
        {
            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder();
            builder.LoadContext = domain;
            builder.Add("public class A{  public int Code = 2; public void Show(){ Console.WriteLine(Code);  }   }", UsingLoadBehavior.WithDefault);
            return builder.GetAssembly();
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
                ReadOnlySpan<byte> newMetadataSpan = new ReadOnlySpan<byte>(newBlob, newLength);
                MetadataUpdater.ApplyUpdate(oldAssembly, newMetadataSpan, Encoding.UTF8.GetBytes(File.ReadAllText("1.txt")), ReadOnlySpan<byte>.Empty);
            }

        }
    }
}
