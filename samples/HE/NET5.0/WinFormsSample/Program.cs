using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
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

            //DS 123
            TestLocalMethod();
            //Once
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Once
            Application.EnableVisualStyles();
            //Once
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            static void TestLocalMethod()
            {
                //DS "In TestLocalMethod"
                Action lambda = () =>
                {
                    //DS "In lambda action"
                    InnerLocalMethod();
                    static void InnerLocalMethod()
                    {
                        Action lambda2 = () =>
                        {


                            //DS "In lambda2"
                            var temp = 6;
                            //DS temp+1
                            //DS temp+1 == 1
                            //DS "Out lambda2"
                        };
                        //DS "In InnerLocalMethod"
                        lambda2();
                        //DS "Out InnerLocalMethod"

                    }
                    //DS "Out lambda action"
                };
                lambda();
                //DS "Out TestLocalMethod"
            }
        }
    }
}
