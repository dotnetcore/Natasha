using PluginBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NatashaFunctionUT
{
    internal static class PluginAssertHelper
    {

        public static (string r1,string r2) GetResult(string path1, string path2, AssemblyCompareInfomation loadBehaviorEnum,bool ignoreBase = true)
        {
            string typeName1 = "P1";
            string typeName2 = typeName1;
            if (Path.GetFileName(path1)!=Path.GetFileName(path2))
            {
                typeName2 = "P2";
            }
            var domain1 =new NatashaDomain(Guid.NewGuid().ToString("N"));
            domain1.SetAssemblyLoadBehavior(loadBehaviorEnum);
            string result1 = string.Empty;
            string result2 = string.Empty;

            Func<AssemblyName, bool>? excludeAssembliesFunc = null;
            if (ignoreBase)
            {
                excludeAssembliesFunc = item => item.Name!.Contains("PluginBase");
            }
           
            var assembly1 = domain1.LoadPlugin(path1, excludeAssembliesFunc);
            try
            {
                var type1 = assembly1.GetTypes().Where(item => item.Name == typeName1).First();
                IPluginBase? plugin1 = (IPluginBase?)Activator.CreateInstance(type1);
                result1 = plugin1!.PluginMethod1();
            }
            catch (Exception ex)
            {
                result1 = ex.GetType().Name;
            }

            try
            {
                var assembly2 = domain1.LoadPlugin(path2, excludeAssembliesFunc);
                var type2 = assembly2.GetTypes().Where(item => item.Name == typeName2).First();
                IPluginBase? plugin2 = (IPluginBase?)Activator.CreateInstance(type2);
                result2 = plugin2!.PluginMethod1();
            }
            catch (Exception ex)
            {
                result2 = ex.GetType().Name;
            }
            try
            {
                var type1 = assembly1.GetTypes().Where(item => item.Name == typeName1).First();
                IPluginBase? plugin1 = (IPluginBase?)Activator.CreateInstance(type1);
                result1 = plugin1!.PluginMethod1();
            }
            catch (Exception ex)
            {

                result1 = ex.GetType().Name;
            }
            return (result1, result2);
        }
    }
}
