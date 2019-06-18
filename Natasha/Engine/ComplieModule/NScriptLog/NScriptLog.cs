namespace Natasha
{

    public static class NScriptLog
    {


        public static bool UseLog;
        static NScriptLog() => UseLog = true;


        public static void Error(string title,string content)
        {
            NScriptLogWriter<NError>.Recoder(title,content);
        }
        public static void Warning(string title, string content)
        {
            NScriptLogWriter<NWarning>.Recoder(title, content);
        }
        public static void Succeed(string title, string content)
        {
            NScriptLogWriter<NSucceed>.Recoder(title, content);
        }
    }


}
