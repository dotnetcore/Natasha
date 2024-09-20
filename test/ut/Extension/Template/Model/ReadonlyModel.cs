using System;
using System.Collections.Generic;
using System.Text;

namespace NatashaUT.Model
{
    public class ReadonlyModel
    {
        private readonly IReadonlyInterface @interface;

        public IReadonlyInterface GetInterface()
        {
            return @interface;
        }

    }

    public interface IReadonlyInterface { }

    public class DefaultReadolyInterface : IReadonlyInterface { }

}
