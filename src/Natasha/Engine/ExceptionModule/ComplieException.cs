using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Natasha
{
    public class ComplieException
    {

        public ComplieException()
        {

            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ComplieError.None;

        }


        public string Source;
        public string Message;
        public string Formatter;
        public ComplieError ErrorFlag;
        public List<Diagnostic> Diagnostics;
       

    }



    public enum ComplieError {
        None,
        Assembly,
        Type,
        Method,
        Delegate
    }

}
