using System.Collections.Generic;

namespace Natasha
{
    public interface IScript
    {
        string Script
        {
            get;
        }

        HashSet<string> Usings
        {
            get;
        }

    }
}