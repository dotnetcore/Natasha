using Natasha.Log.Model;

namespace Natasha.Log
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
