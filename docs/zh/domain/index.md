<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/domain.html">English</a>
</p>  


## 域操作

程序域也称作程序集上下文，是程序集的载体，我们可以创建域用来隔离代码环境，也可以卸载域以保证系统资源释放与回收，
我们还可以使用锁域的方法来保证当前使用环境为被锁住的域。
用法如下：

```C#

//创建一个域
DomainManagment.Create("MyDomain");
//移除一个域
DomainManagment.Remove("MyDomain");
//判断域是否被弱引用所删除(被GC回收)
DomainManagment.IsDeleted("MyDomain");
//获取一个ALC上下文
DomainManagment.Get("MyDomain");




//锁住域上下文
using(DomainManagment.Lock("MyDomain"))
{

    var domain = DomainManagment.CurrentDomain;
    //code in 'MyDomain' domain 
    
}


//创建并锁定一个域上下文
using(DomainManagment.CreateAndLock("MyDomain"))
{

    var domain = DomainManagment.CurrentDomain;
    //code in 'MyDomain' domain 
    
}

```
