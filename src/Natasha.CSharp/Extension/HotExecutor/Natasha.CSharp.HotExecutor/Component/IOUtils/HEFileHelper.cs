using Natasha.CSharp.Extension.HotExecutor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component
{
    public static class HEFileHelper
    {
        public async static Task<string> ReadUtf8FileAsync(string file)
        {
            if (!File.Exists(file))
            {
#if DEBUG
                HEProxy.ShowMessage($"不存在文件：{file}");
#endif
                return string.Empty;
            }
            StreamReader stream;
            do
            {
                try
                {
                    stream = new(file, Encoding.UTF8);
                    var content = await stream.ReadToEndAsync();
                    stream.Dispose();
                    return content;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("命中文件读锁！");
                    Thread.Sleep(200);
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
                }
                catch (Exception)
                {
                    Debug.WriteLine("命中文件写锁！");
                    Thread.Sleep(200);
                }


            } while (true);
        }
    }
}
