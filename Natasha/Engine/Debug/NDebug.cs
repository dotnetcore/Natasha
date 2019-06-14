namespace Natasha
{

    public static class NDebug
    {
        public static bool UseLog;
        public static void Error(string title,string content)
        {
            NDebugWriter<NError>.Recoder(title,content);
        }
        public static void Warning(string title, string content)
        {
            NDebugWriter<NWarning>.Recoder(title, content);
        }
        public static void Succeed(string title, string content)
        {
            NDebugWriter<NSucceed>.Recoder(title, content);
        }
    }


}
