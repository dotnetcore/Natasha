using System.Text.Json;

namespace Github.NET.Sdk
{
    public static class RecommendHelper
    {
        private static readonly string _compareFile;
        private static readonly string _recommendResultFile;
        private static readonly JsonSerializerOptions _serializerOptions;

        static RecommendHelper()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _compareFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recommend", "compare.json");
            _recommendResultFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recommend", "recommend.json");
        }
        public static async Task<(RecommendResultModel[]?, string)> RecommendAsync(string compareJson, IEnumerable<RecommendInfoModel> recommendInfos, List<(int count, double min, double max)> pickInfo)
        {
            RecommendCompareModel compareModel = new()
            {
                Source = compareJson,
                Reference = recommendInfos
            };

            int compareCount = 0;
            foreach (var item in pickInfo)
            {
                compareCount += item.count;
                compareModel.PickCount.Add(compareCount);
                compareModel.PickMinValues.Add(item.min);
                compareModel.PickMaxValues.Add(item.max);
            }
            File.WriteAllText(_compareFile, JsonSerializer.Serialize(compareModel, _serializerOptions));
            if (!File.Exists(_compareFile))
            {
                return (null, $"未找到 recommend.py 执行文件！信息：不存在路径{_compareFile}");
            }

            var pyExecuteResult = await CLIHelper.ExecuteBashCommand("python recommend.py", Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Recommend"));
            if (!File.Exists(_recommendResultFile))
            {
                return (null, $"未找到相似度结果文件！Python 任务执行失败: {pyExecuteResult.Item2}{Environment.NewLine}源字串: {JsonSerializer.Serialize(compareModel, _serializerOptions)}");
            }
            else
            {
                var resultContent = File.ReadAllText(_recommendResultFile);
                var results = JsonSerializer.Deserialize<RecommendResultModel?[]?>(resultContent, _serializerOptions);
                if (results != null)
                {
                    return (results!.Where<RecommendResultModel>(item => item != null).ToArray(), resultContent);
                }
                else
                {
                    return (null, $"Python 结果和序列化可能出现了问题!{resultContent}");
                }
            }
        }
    }
}
