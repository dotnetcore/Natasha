using Natasha.Log;
using Natasha.Log.Model;

namespace System
{

    public class NWarningLog : NatashaLogBase
    {

        public static bool Enabled;

        static NWarningLog() => Enabled = false;
        



        public override void Write()
        {
            NatashaWriterIniter<NWarningLog>.Recoder(Buffer);
        }




        public void Handler(string str)
        {
            Buffer.AppendLine(str);
        }

    }

}
