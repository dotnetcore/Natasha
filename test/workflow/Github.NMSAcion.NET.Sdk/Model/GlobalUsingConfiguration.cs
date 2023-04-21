namespace Github.NMSAcion.NET.Sdk.Model
{
    public sealed class GlobalUsingConfiguration
    {
        public bool Enable { get; set; }

        public HashSet<string>? Ignores { get; set; }
    }
}
