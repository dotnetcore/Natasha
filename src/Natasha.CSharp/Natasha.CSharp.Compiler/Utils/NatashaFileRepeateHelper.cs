using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Natasha.CSharp.Compiler.Utils
{
    internal static class NatashaFileRepeateHelper
    {
        public static string GetAvaliableFilePath(string file)
        {
            var tempOutputFolder = Path.GetDirectoryName(file);
            if (!Directory.Exists(tempOutputFolder))
            {
                Directory.CreateDirectory(tempOutputFolder);
            }
            var tempFileName = Path.GetFileName(file);
            return Path.Combine(tempOutputFolder, $"repeate.{Guid.NewGuid():N}.{tempFileName}");
        }
    }
}
