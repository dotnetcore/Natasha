using Github.NET.Sdk;

namespace ReferenceSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var name = Assembly.GetAssembly(null).GetName().GetUniqueName();
            var projects = SolutionInfo.GetCSProjectsByStartFolder("src");
            foreach (var item in projects.Projects)
            {
                Console.WriteLine(item.ProjectName + item.PackageName+":"+string.Join(',', item.TargetFramworks)+": iSpACK:"+item.IsPackable+":"+item.RelativeFolder);
            }
            //NatashaManagement.Preheating();
            Console.ReadKey();
        }
    }
}