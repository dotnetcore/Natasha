using Natasha.CSharp.Extension.HotExecutor;
using System.Diagnostics;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component
{
    public class HEFileLogger
    {
        private readonly HESpinLockHelper _fileLock = new();
        //private readonly StreamWriter _writer;
        private readonly string _file;
        public HEFileLogger(string file)
        {
            var folder = Path.GetDirectoryName(file);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            _file = file;

        }

        public async Task WriteUtf8FileAsync(string msg)
        {
            bool isWritten = false;
            _fileLock.GetAndWaitLock();
            do
            {
                try
                {
                    using StreamWriter _writer = new(_file, true, Encoding.UTF8);
                    if (_writer.BaseStream.CanWrite)
                    {
                        await _writer.WriteLineAsync(msg);
                        isWritten = true;
                        await _writer.FlushAsync();
                    }
                    _fileLock.ReleaseLock();
                    return;
                }
                catch (Exception)
                {
                    Debug.WriteLine("命中 HE 日志写锁！");
                    Thread.Sleep(200);
                }

            } while (!isWritten);
        }
    }
}
