using System;

namespace Natasha.Remote
{
    public class RemoteBuilder
    {
        public static string Script
        {
            set
            {
                Type type = RuntimeComplier.GetType(value);
                RemoteWritter.ComplieToRemote(type);
            }
        }
    }
}
