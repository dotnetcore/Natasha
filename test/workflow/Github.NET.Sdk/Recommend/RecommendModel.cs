namespace Github.NET.Sdk
{

    public class RecommendInfoModel
    {
        public string Id { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
    public sealed class RecommendResultModel : RecommendInfoModel
    {
        public string Output { get; set; } = string.Empty;
    }

    public sealed class RecommendCompareModel
    {
        public List<int> PickCount { get; set; } = new();
        public List<double> PickMinValues { get; set; } = new();
        public List<double> PickMaxValues { get; set; } = new();
        public string Source { get; set; } = string.Empty;
        public IEnumerable<RecommendInfoModel> Reference { get; set; } = Array.Empty<RecommendInfoModel>();
    }
}
