using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class NUsing
    {

        internal List<NamespaceConverter> _using_cache;
        public NUsing()
        {

            _using_cache = new List<NamespaceConverter>();

        }


        public void AddUsing(NamespaceConverter @using)
        {

            _using_cache.Add(@using);

        }




        public void RemoveUsing(NamespaceConverter @using)
        {

            _using_cache.Remove(@using);

        }

    }

}
