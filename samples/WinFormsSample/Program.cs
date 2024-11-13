using Natasha.CSharp.HotExecutor.Component;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace WinFormsSample
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //Once
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());


            //DS "A"
            for (int i = 0; i < 6; i++)
            {
                //DS "GC";
                GC.Collect();
            }
            foreach (var item in DomainManagement.Cache)
            {
                if (item.Value!=null)
                {
                    var A = 1;
                    //DS (item.Key + " ´æ»î£¡")

                }
            }

            //DS DomainManagement.Cache.Count

        }
    }
}