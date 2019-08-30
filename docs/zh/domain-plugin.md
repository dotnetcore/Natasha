### 域内添加插件

```C#

//向域中注入插件 
string dllPath = @"1/2/3.dll";
var domain = DomainManagment.Get/Create("MyDomain");
var assembly = domain.LoadFile(dllPath);



//锁域与插件解构操作
string dllPath = @"1/2/3.dll";
using(DomainManagment.CreateAndLock("MyDomain"))
{
    var (Assembly,TypeCache) = dllPath;
    //Assembly: Assembly
    //TypeCache: ConcurrentDictionary<string,Type> 
}



//将引用从当前域内移除，下次编译将不会带着该程序集的信息
domain.RemoveDll(dllPath);
domain.RemoveAssembly(assembly);
domain.RemoveType(type);

```
