using System;
using System.Collections.Generic;
using System.Text;

namespace NatashaBenchmark.Model
{
    public class OtherNameSpaceMethod
    {
        public string FromDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM");
        }
    }
}
