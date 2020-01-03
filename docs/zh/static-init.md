
<p align="center">
 <a href="https://natasha.dotnetcore.xyz/"> 返回 </a> |  <a href="https://natasha.dotnetcore.xyz/en/static-init.html">English</a>
</p>  


## 静态初始化操作

Natasha 几乎所有的操作类都严格遵守域先行，配置优先的原则。我们在操作类中封装了以下几种静态初始化函数：

| 参数名 | 默认值 | 行为 |
|--------- |------------- | ---- |
| domainName | 需要传入 | 如果 domainName 的值为空或者default, 则默认使用系统域. |
| target | ComplierResultTarget.Stream | 将动态内容编译到内存. |
| error | ComplierResultError.None | 编译出错时不会引发异常. |

<br/>

## 使用

 NDomain / NAssembly / XXXOerator 等等以下称为 “Handler”.
 
 #### 静态初始化一 Create 操作：

```C#

//创建一个 "domainJim" 域
//target 使用默认值
//error 使用默认值。
Handler.Create("domianJim");


//创建一个 "domainJim" 域
//动态内容编译到 dll 文件中
//error 使用默认值。
Handler.Create("domianJim", ComplierResultTarget.File);


//创建一个 "domainJim" 域
//target 使用默认值
//出错时引发异常。
Handler.Create("domianJim", ComplierResultError.ThrowException);

```
#### 静态初始化二 Default 操作：

```C#

//创建一个系统域
//target 使用默认值
//error 使用默认值。
Handler.Default();


//创建一个系统域
//target 动态内容编译到 dll 文件中 
//出错时引发异常。
Handler.Default(ComplierResultError.ThrowException, ComplierResultTarget.File);
Handler.Default(ComplierResultTarget.File, ComplierResultError.ThrowException);

```

#### 静态初始化三 Random 操作:

```C#

//随机创建一个域
//target 使用默认值
//error 使用默认值。
Handler.Random();


//随机创建一个域
//target 动态内容编译到 dll 文件中 
//出错时引发异常。
Handler.Random(ComplierResultError.ThrowException, ComplierResultTarget.File);
Handler.Random(ComplierResultTarget.File, ComplierResultError.ThrowException);

```
