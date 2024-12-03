using System.Diagnostics;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Utils
{
    public static class HEFileHelper
    {
        public async static Task<string> ReadUtf8FileAsync(string file)
        {
            do
            {
                try
                {
                    if (File.Exists(file))
                    {
                        using StreamReader stream = new(file, Encoding.UTF8);
                        return await stream.ReadToEndAsync();
                    }
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("命中文件读锁！");
                    await Task.Delay(200);
                }


            } while (true);

        }
        public async static Task WriteUtf8FileAsync(string file, string msg)
        {
            do
            {
                try
                {
                    using StreamWriter stream = new(file, true, Encoding.UTF8);
                    await stream.WriteLineAsync(msg);
                    await stream.FlushAsync();
                    return;
                }
                catch (Exception)
                {
                    Debug.WriteLine("命中文件写锁！");
                    await Task.Delay(200);
                }


            } while (true);
        }
    }
}
