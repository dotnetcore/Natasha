using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natasha.Core
{
    public interface IOperator
    {
        void RunCompareAction();
        void AddSelf();
        void SubSelf();
    }
}
