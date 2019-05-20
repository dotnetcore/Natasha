using System;

namespace Natasha.Remote
{
    public class RemoteBuilder
    {
        private string _script;
        public string Script
        {
            get { return _script; }
            set
            {
                Type type = ClassBuilder.GetType(value);
                RemoteWritter.ComplieToRemote(type);
            }
        }
    }
}
