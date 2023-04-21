using Github.NET.Sdk;
using System.Text.RegularExpressions;

internal static class ResourcesHelper
{
    public static (string owner, string repo) GetCurrentRepository()
    {
        /*
         * [remote "origin"]
	        url = https://github.com/night-moon-studio/Template.git
         */
        Regex reg = new(".*http.*/(?<owner>.*?)/(?<repo>.*?)\\.git", RegexOptions.Singleline);
        var content = File.ReadAllText(Path.Combine(SolutionInfo.Root, ".git", "config"));
        var match = reg.Match(content);
        if (match.Success)
        {
            var owner = match.Groups["owner"].Value;
            var repo = match.Groups["repo"].Value;
            return (owner, repo);
        }
        return (string.Empty, string.Empty);
    }

}

