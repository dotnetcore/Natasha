using System.Text.RegularExpressions;

namespace Publish.Helper
{
    public static class ChangeLogHelper
    {
        private static readonly Regex _packagePicker;
        private static readonly Regex _publishPicker;
        static ChangeLogHelper()
        {
            //## [1.0.0] - 2022-06-09
            //###  Nms.Template.Test _ v1.0.0:
            //-初步版本 简单实现 还未优化
            _publishPicker = new Regex("^.*##[^\n#]+?\\[(?<releaseVersion>\\d+.\\d+.\\d+.*?)\\].*?\n(?<result>.*?)$", RegexOptions.Singleline | RegexOptions.Compiled);
            _packagePicker = new Regex("###[\\s]*?(?<name>.*?)[\\s]*?_[\\s]*?(v(?<version>.*?)|(?<version>[^\\s]*?))[:：：]", RegexOptions.Compiled);
        }

        public static (string version,string log) GetLatestReleaseInfo(string content)
        {
            var match = _publishPicker.Match(content);
            if (match.Success)
            {
                return (match.Groups["releaseVersion"].Value, match.Groups["result"].Value);
            }
            return (string.Empty,string.Empty);
        }

        public static (string name,string version)[] GetReleasePackageInfo(string content)
        {
            var matches = _packagePicker.Matches(content);
            if (matches.Count>0)
            {
                var result = new (string name, string version)[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                {
                    result[i] = (matches[i].Groups["name"].Value.Trim(), matches[i].Groups["version"].Value.Trim());
                }
                return result;
            }
            return Array.Empty<(string name, string version)>();
        }

        public static (string version, string log) GetReleaseInfoFromFromFile(string filePath)
        {
            var content = File.ReadAllText(filePath);
            return GetLatestReleaseInfo(content);
        }
    }
}
