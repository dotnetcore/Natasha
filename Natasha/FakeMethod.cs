using System;
using System.Reflection;

namespace Natasha
{
    public class FakeMethod:MethodBuilder<FakeMethod>
    {
        public Action<MethodTemplate> Action;
        public static FakeMethod New
        {
            get { return new FakeMethod(); }
        }
        private MethodInfo _temp_info;
        public FakeMethod()
        {
            _link = this;
            ClassTemplate
                .HiddenNameSpace()
                .Access(AccessTypes.Public);
        }

        public FakeMethod UseMethod(MethodInfo info)
        {
            _temp_info = info;
            return this;
        }

        public FakeMethod ModifyAction(Action<MethodTemplate> action)
        {
            Action = action;
            return this;
        }
        public FakeMethod MethodBody(string content)
        {
            Action?.Invoke(MethodTemplate);
            if (!MethodTemplate.HashMethodName())
            {
                MethodTemplate.Name(_temp_info);
            }
            MethodTemplate
                     .Access(_temp_info)
                     .Modifier(_temp_info)
                     .Parameter(_temp_info)
                     .Body(content)
                     .Return(_temp_info);
            _info = MethodTemplate.Package();
            return this;
        }
    }
}
