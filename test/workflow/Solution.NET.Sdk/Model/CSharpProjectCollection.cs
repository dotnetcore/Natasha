namespace Solution.NET.Sdk.Model
{
    public sealed class CSharpProjectCollection
    {
        public CSharpProject? GlobalConfig { get; set; }

        public List<CSharpProject> Projects { get; set; } = new List<CSharpProject>();

    }
}
