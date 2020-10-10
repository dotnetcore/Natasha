using Natasha.Log;
using Natasha.Log.Model;

namespace System
{

    public class NWarningLog : ALogWrite
    {

        public static bool Enabled;

        static NWarningLog() => Enabled = false;
        



        public override void Write()
        {
            NWriter<NWarningLog>.Recoder(Buffer);
        }




        public void Handler(string str)
        {
            Buffer.AppendLine(str);
        }

    }

}
