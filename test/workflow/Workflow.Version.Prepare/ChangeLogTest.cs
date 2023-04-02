using Publish.Helper;

namespace WorkflowPrepare
{
    [Trait("管道功能", "发布日志")]
    public class ChangeLogTest
    {
        [Fact(DisplayName = "版本扫描")]
        public async Task ScanTest()
        {
            bool isWriteToOutPut = false;
            var (version, log) = ChangeLogHelper.GetReleaseInfoFromFromFile(ResourcesHelper.ChangeLogFile);
            if (!OperatingSystem.IsWindows())
            {
                isWriteToOutPut = await CLIHelper.Output("RELEASE_VERSION", version);
            }
            Assert.True(isWriteToOutPut);
            var files = Directory.GetFiles(ResourcesHelper.CurrentSrcFolder);
            Assert.True(files.Length > 0);
        }
    }
}