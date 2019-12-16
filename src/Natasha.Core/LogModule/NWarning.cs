using Natasha.Log.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Log
{

    public class NWarning : ALogWrite
    {

        public static bool Enabled;

        static NWarning() => Enabled = true;
        



        public override void Write()
        {
            NWriter<NWarning>.Recoder(Buffer);
        }




        public void Handler(string str)
        {
            Buffer.Append(str);
        }

    }

}
