using System;

namespace Natasha.Remote
{
    public class RemoteBuilder
    {
        public static string Script
        {
            set
            {
                OopComplier oop = new OopComplier();
                Type type = oop.GetClassType(value);
                RemoteWritter.ComplieToRemote(type);
            }
        }
    }
}
