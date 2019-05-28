using Natasha.Builder;
using System;
using System.Reflection;

namespace Natasha
{
    public class FakeMethod : OnceMethodBuilder<FakeMethod>
    {
        public Action<MethodTemplate> Action;
        public static FakeMethod New
        {
            get { return new FakeMethod(); }
        }
        private MethodInfo _temp_info;
        public FakeMethod()
        {
            Link = this;
            HiddenNameSpace();
            ClassAccess(AccessTypes.Public);
        }

        public FakeMethod UseMethod(MethodInfo info)
        {
            _temp_info = info;
            return this;
        }

        public FakeMethod MethodContent(string content)
        {
            if (!HashMethodName())
            {
                MethodName(_temp_info);
            }
            MethodAccess(_temp_info)
            .MethodModifier(_temp_info)
            .Parameter(_temp_info)
            .MethodBody(content)
            .Return(_temp_info)
            .Builder();
            return this;
        }
    }
}
