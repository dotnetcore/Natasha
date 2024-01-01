namespace Natasha.CSharp.Codecov.Utils
{
    public class CodecovRecorder(string name, string mvid, int count)
    {
        public readonly string Mvid = mvid;
        public readonly string AssemblyName = name;
        public readonly bool[][] UsageCache = new bool[count][];
        public readonly string[] MethodNameCache = new string[count];

        public bool[] CreatePayload(Guid mvid, int methodToken, int fileIndex, ref bool[] payload, int payloadLength)
        {
            if (Interlocked.CompareExchange(ref payload, new bool[payloadLength], null) == null)
            {
                UsageCache[methodToken - 1] = payload;
                return payload;
            }
            return UsageCache[methodToken - 1];
        }

        public bool[] CreatePayload(Guid mvid, int methodToken, int[] fileIndices, ref bool[] payload, int payloadLength)
        {
            throw new NotSupportedException();
        }

        public void FlushPayload()
        {
            for (int i = 0; i < UsageCache.Length; i++)
            {
                bool[] payload = UsageCache[i];
                if (payload != null)
                {
                    payload.AsSpan().Fill(false);
                }
            }
        }

        public List<(string, bool[])> ToList()
        {
            List<(string, bool[])> result = [];
            for (int i = 0; i < MethodNameCache.Length; i++)
            {
                if (!MethodDefinedFilter.IsMatch(MethodNameCache[i]))
                {
                    result.Add((MethodNameCache[i], UsageCache[i]));
                }
            }
            return result;
        }
    }
}
