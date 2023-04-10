namespace Github.NET.Sdk.Model
{
    public sealed class ProjectV2SingleSelectField
    {
        public string Id { get; set; } = string.Empty;
        public ProjectV2SingleSelectOptions[]? Options { get; set; }
    }

    public sealed class ProjectV2SingleSelectOptions 
    { 
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

}
