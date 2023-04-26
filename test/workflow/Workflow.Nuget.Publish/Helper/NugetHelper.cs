using Github.NET.Sdk;
using NuGet.Versioning;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;

namespace Publish.Helper
{
    internal static class NugetHelper
    {
        private static readonly HttpClient _nugetClient;

        static NugetHelper()
        {
            _nugetClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(60)
            };

        }

        public static async Task<NuGetVersion?> GetLatestPreviewVersionAsync(string packageName)
        {
            return await GetLatestVersionAsync(packageName, false);
        }
        public static async Task<NuGetVersion?> GetLatestStableVersionAsync(string packageName)
        {

            return await GetLatestVersionAsync(packageName, true);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static async Task<NuGetVersion?> GetLatestVersionAsync(string packageName, bool? isPre = null)
        {

            var versionWrapper = await GetVersionsAsync(packageName, isPre);
            if (versionWrapper != null)
            {
                var latestVersion = versionWrapper.Max();
                Console.WriteLine("最后版本:" + latestVersion);
                return latestVersion;
            }

            return null;
        }





        public static async Task<IEnumerable<NuGetVersion>?> GetStableVersionsAsync(string packageName)
        {
            return await GetVersionsAsync(packageName, false);
        }
        public static async Task<IEnumerable<NuGetVersion>?> GetPreviewVersionsAsync(string packageName)
        {
            return await GetVersionsAsync(packageName, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static async Task<IEnumerable<NuGetVersion>?> GetVersionsAsync(string packageName, bool? isPre = null)
        {
            var requestUrl = $"https://api.nuget.org/v3-flatcontainer/{packageName.ToLowerInvariant()}/index.json";

            var notFound404 = 3;

            HttpResponseMessage response;
            while (true)
            {
                try
                {
                    response = await _nugetClient.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var versions = await response.Content.ReadFromJsonAsync<NugetVersionModel>();
                            if (isPre.HasValue)
                            {
                                if (isPre.Value)
                                {
                                    return versions!.versions.Where(item => item.Contains('-')).Select(item => new NuGetVersion(item)).ToList();
                                }
                                else
                                {
                                    return versions!.versions.Where(item => !item.Contains('-')).Select(item => new NuGetVersion(item)).ToList();
                                }

                            }
                            else
                            {
                                return versions!.versions.Select(item => new NuGetVersion(item)).ToList();
                            }
                        }
                        catch (Exception ex)
                        {
                            return null;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        notFound404 -= 1;
                        if (notFound404 == 0)
                        {
                            return null;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                catch
                {

                }
                await Task.Delay(2000);
            }
        }


        internal sealed class NugetVersionModel
        {
            //public NuGetVersion[] versions { get; set; } = default!;
            public string[] versions { get; set; } = default!;
        }


        public static async ValueTask<bool> BuildAsync(string csprojFile)
        {

            bool result = true;
            using Process process = new();
            var info = process.StartInfo;
            info.FileName = "dotnet";
            info.WorkingDirectory = SolutionInfo.Root;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.StandardErrorEncoding = Encoding.UTF8;
            info.StandardOutputEncoding = Encoding.UTF8;
            process.StartInfo = info;
            process.OutputDataReceived += (sender, e) =>
            {
#if DEBUG
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine(e.Data);
                }
#endif
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    result = false;
#if DEBUG
                    Console.WriteLine(e.Data);
#endif
                }
            };

            info.Arguments = $"build {csprojFile} --nologo -c Release";
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();
            process.Kill();
            return result;
        }


        public static async ValueTask<bool> PackAsync(string csprojFile)
        {
            bool result = true;
            using Process process = new();
            var info = process.StartInfo;
            info.FileName = "dotnet";
            info.WorkingDirectory = SolutionInfo.Root;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.StandardErrorEncoding = Encoding.UTF8;
            info.StandardOutputEncoding = Encoding.UTF8;
            process.StartInfo = info;
            process.OutputDataReceived += (sender, e) =>
            {
#if DEBUG
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine(e.Data);
                }
#endif
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    result = false;
#if DEBUG
                    Console.WriteLine(e.Data);
#endif
                }
            };


            info.Arguments = $"pack --include-symbols -p:SymbolPackageFormat=snupkg --nologo --no-build --no-restore -c Release {csprojFile} -o .";
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();
            process.Kill();
            return result;
        }


    }
}
