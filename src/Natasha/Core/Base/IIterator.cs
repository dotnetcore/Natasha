using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Core
{
    //迭代接口
    public interface IIterator
    {
        MethodInfo MoveNext { get; set; }
        MethodInfo Current { get; set; }
        MethodInfo Dispose { get; set; }
        LocalBuilder TempEnumerator { get; set; }
        int Length { get; set; }

        void Initialize();
        void LoadCurrentElement(LocalBuilder currentBuilder);
    }
}
