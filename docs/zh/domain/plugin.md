### 域内添加插件

```C#

//向域中注入插件 
string dllPath = @"1/2/3.dll";
var domain = DomainManagment.Get/Create("MyDomain");

//以文件方式加载插件
//3.0版本会进行deps.json的依赖文件监测
var assembly = domain.LoadPluginFromFile(dllPath);
//以流的方式加载插件
var assembly = domain.LoadPluginFromStream(dllPath);



//将引用从当前域内移除，下次编译将不会带着该程序集的信息

//移除短名引用
domain.Remove(dllPath);
//移除程序集引用，或者短名引用
domain.Remove(assembly);

```
