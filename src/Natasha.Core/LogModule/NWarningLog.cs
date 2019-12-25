using Natasha.Log.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Log
{

    public class NWarningLog : ALogWrite
    {

        public static bool Enabled;

        static NWarningLog() => Enabled = true;
        



        public override void Write()
        {
            NWriter<NWarningLog>.Recoder(Buffer);
        }




        public void Handler(string str)
        {
            Buffer.Append(str);
        }

    }

}
