# Natasha

简化IL操作，优化IL编程流程，编写提供高性能的动态缓存，像写普通代码一样去写IL代码。

原文：[开源库 Natasha2016 ，让IL编程跑起来](http://blog.csdn.net/LanX_Fly/article/details/59617050)

版权声明：本文为博主（LanX_Fly）原创文章，未经博主允许不得转载。

## 背景：

IL编程在普通的程序员的代码里几乎不会出现，但从Json.net、Dapper、Asp.net等等开源项目都体现出了IL编程的重要性。

在IL编程的时候，上百行甚至上千行的IL代码实在让人头大，调试不方便不说，IL编程的逻辑也是不同于以往的编程。
为了简化操作，优化IL编程逻辑，设计了这个库，取名为Natasha.

## 简介：

类库名称：Natasha (娜塔莎)（原型苏联红军第25步兵师的中尉柳德米拉·帕夫利琴科，一名出色的女狙击手）

开源协议：MPL2.0

版    本：2016

作    者：LanX

感    谢：Xue 和 Denni 在测试以及审查代码上的支持

地 址：[https://github.com/dotnetcore/Natasha](https://github.com/dotnetcore/Natasha)

API 文档地址：[测试地址]()

## 类库功能：

* 动态方法

    * 支持多线程创建动态方法；

    * EMethod:对任意类方法的操作类；

    * EHandler:创建动态方法的操作类；

* 普通变量EVar

    * 对int、double、bool、float、string…等变量的操作以及枚举类型的若干操作

    * 重载一元二元运算符提供各种变量(包括运行时)的 + - * / % | & >> << ++ –操作；

    * 重载比较运算符提供各种变量(包括运行时)的比较操作;

    * 实现ILoadInstance数据入栈接口;

    * 临时变量可自加减以及Store赋值操作;

    * 可创建无临时变量的操作实例;

* 类和结构体EModel

    * 支持三种常规压栈方式(Builder,Action,Parameter);

    * 实现ILoadInstance数据入栈接口(this入栈);

    * 支持属性、字段、方法、私有属性字段（结构体不支持）的及时操作以及延迟操作;

    * 重载一元二元运算符提供各种变量(包括运行时)的 + - * / % | & >> << 操作;

    * 重载比较运算符提供各种变量(包括运行时)的比较操作;

    * 支持链式加载或延迟加载嵌套类或结构体的属性、字段、方法;

    * 提供链式操作来操作方法返回来的对象且无临时变量;

    * 支持CreateModelFromObject方法进行深度压栈复制，保证对象全新;

    * 提供EstruckCheck类检测结构体是否为刚初始化的结构体；注：结构体初始化不是空;

* 数组EArray

    * 支持三种常规压栈方式(Builder,Action,Parameter);

    * 实现ILoadInstance数据入栈接口(this入栈);

    * 提供StoreArray方法，支持各种数据入栈填充数组;

    * 提供LoadArray函数加载指定索引出的元素入栈;

    * 支持CreateArrayFromRuntimeArray方法进行深度压栈复制保证对象全新（直接等于也行）;

* 条件分支EJudge

    * 提供了if elseif else三个函数对逻辑进行分支处理;

    * 提供了 TrueJump正确跳转函数，FalseJump 错误跳转函数，JumpTo无条件跳转函数;

    * ENull提供了空值检测;

    * EDefult提供了默认值检测;

    * EDBNull提供了对DBNull类型检测;

* 循环结构ELoop

    * 提供了对数组的遍历操作;

    * 保留了对迭代器遍历的方法;

    * 提供了while循环操作;

* 拆装箱EPacket

    * EPacket提供了装箱函数以及拆箱函数

* 自定义类ClassBuilder

    * 支持自定义类，自定义属性，字段，方法及其保护级别等功能

    * 支持EModel使用自定义类

## 用前科普

* 动态方法IL环境:

    ```
    我们创建动态方法时需要DynamicMethod对象创建ILGenerator实例来操作动态方法。 

    比如 ： 

    ilA = methodA.GetILGenerator(); 

    ilB = methodB.GetILGenerator(); 

    当你用ilA来写IL代码的时候，不能用ilB来参与methodA的创建过程。 

    也就是ilA产生的临时变量不能用在ilB中，ilA与ilB是相互隔离的。 

    我们在自定义类的时候也会有新的il产生，在设置属性的get/set方法的时候也会有新的il产生。 

    这里还涉及到在动态方法中创建动态方法，所以需要保证il的工作环境相对是隔离的。 

    因此设计了ThreadCache缓存类来设置和获取不同环境的IL或者action。 

    EVar提供了普通变量的操作， EClass提供了类和结构体的操作，EArray提供了数组的操作这三个主要的操作类都有共同的基类ILGeneratorBase，初始化时会从ThreadCache中取IL.
    ```

* 类的入栈过程

    this.Age = 1:

    * 将this入栈。
    * 将1入栈。
    * 填充fieldinfo 或者 propertyinfo。

    this.Show()：

    * 将this入栈。
    * 调用show的methodinfo

    this指针入栈：

    * 委托形式
    * 外面的LocalBuilder
    * 参数形式

    在IL编程中：

    * LocalBuilder是临时变量，用Ldloc代码来调用;
    * ndex指定参数索引，用Ldarg等IL代码来调用;
    * 托中的代码比前两种更复杂;

    因此引出初始化类的几个初始化方法： 

    private EModel(Type type, Action action): base(action, type){} 

    private EModel(LocalBuilder builder, Type type): base(builder, type) {} 

    private EModel(int parameterIndex, Type type): base(parameterIndex, type){}

    不管上面三种你选择了哪一种，this入栈都是由以下的接口来完成的。 

    还有ILoadInstance接口： 

    void Load(); //不是结构体或者结构体作为值类型填充时，入栈 

    void LoadAddress(); //如果是结构体得加载地址 

    注:实际封装要比这三种多，详情请看工程里的例子

## 使用方法

* 创建动态方法:
``` csharp
//动态创建Action委托
Delegate newMethod0 = EHandler.CreateMethod<ENull>((il)=>{

}).Compile();

//动态创建Action<string,int>委托
Delegate newMethod1 = EHandler.CreateMethod<string,int,ENull>((il)=>{

}).Compile();

//动态创建Func<string>委托
Delegate newMethod2 = EHandler.CreateMethod<string>((il) => { 

}).Compile();

//动态创建Func<string,TestClass>委托
Delegate newMethod3 = EHandler.CreateMethod<string, TestClass>((il) =>{

}).Compile();
```

* 创建普通变量(非结构体，非类实例，非数组):
``` csharp
//动态创建Action委托
Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
{
      //创建没有临时变量的普通入栈变量(没有临时变量，所以自加操作没有意义)
      EVar intHandler = 1;

      //创建函数操作句柄
      EMethod method = typeof(Console);

      //输出intHandler的时候，让变量做加法运算。
      method.ExecuteMethod<int>("WriteLine", intHandler + 665);

      //结果:666；

}).Compile();
((Action)newMethod)();

//动态创建Action委托
Delegate newMethod1 = EHandler.CreateMethod<ENull>((il) =>
{
       //创建有临时变量的普通入栈变量(自加操作可以被自身储存) 也就是说可以使用store存储函数
       //int i = 664;
       EVar intHandler = EVar.CreateVarFromObject(664);

       //i++;
       intHandler++;

       //i=i+1;
       intHandler.Store(intHandler + 1);

       //创建函数操作句柄
       EMethod method = typeof(Console);

       //输出intHandler
       method.ExecuteMethod<int>("WriteLine", intHandler);

       //结果:666

 }).Compile();
((Action)newMethod1)();
```

* 创建类\创建结构体:
``` csharp
//动态创建Action委托
Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
{

    EModel model = null;

    //测试类的字段
    //model = EModel.CreateModel<DocumentClassFieldTest>();

    //测试类的属性
    //model = EModel.CreateModel<DocumentClassPropertyTest>();

    //测试结构体的字段
    model = EModel.CreateModel<StructField>();

    //测试结构体的属性
    // model = EModel.CreateModel<DocumentStructPropertyTest>();

    model.Set("PublicName", "This is Public-Name");
    model.Set("PrivateName", "This is Private-Name");
    model.Set("PublicAge", 666);
    model.Set("PrivateAge", 666);

    EMethod method = typeof(Console);
    method.ExecuteMethod<string>("WriteLine", model.Load("PrivateName"));
    method.ExecuteMethod<string>("WriteLine", model.Load("PublicName"));
    method.ExecuteMethod<int>("WriteLine", model.Load("PublicAge"));
    method.ExecuteMethod<int>("WriteLine", model.Load("PrivateAge"));

}).Compile();
((Action)newMethod)();
```

* 创建数组\遍历数组:
``` csharp
string[]        strArray    = new string[5];
StructField[]   structArray = new StructField[5];
DocumentEnum[]  enumArray   = new DocumentEnum[5];

for (int i = 0; i < strArray.Length; i += 1)
{
    strArray[i] = i.ToString();
    enumArray[i] = DocumentEnum.ID;
    structArray[i] = new StructField() { PublicAge = i };
}

//string数组遍历输出
Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
{
    EMethod method = typeof(Console);

    //从运行时获取数组并入栈到IL层临时变量
    EArray stringArrayModel = strArray;

    //遍历数组
    ELoop.For(stringArrayModel, (currentElement) =>
    {
        method.ExecuteMethod<string>("WriteLine", currentElement);
    });

}).Compile();
((Action)newMethod)();


//自定义数组
Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
{
    //创建长度为10的int类型数组
    EArray arrayModel = EArray.CreateArraySpecifiedLength<int>(10);

    //索引为5，6,7处填充数据6
    arrayModel.StoreArray(5, 6);
    arrayModel.StoreArray(6, 6);
    arrayModel.StoreArray(7, 6);

    //遍历输出
    ELoop.For(0, 10, 1, arrayModel, (currentElement) =>
    {
        method.ExecuteMethod<int>("WriteLine", currentElement);
    });
}).Compile();
((Action)newMethod)();

//结构体数组遍历输出
Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
{
    //从运行时获取数组并入栈到IL层临时变量 或者EArray structArrayModel = structArray
    EArray structArrayModel = EArray.CreateArrayFromRuntimeArray(structArray);

    //遍历输出元素的PublicAge字段
    ELoop.For(structArrayModel, (currentElement) =>
    {
        EModel model = EModel.CreateModelFromAction(currentElement, typeof(StructField));
        model.LField("PublicAge");
        method.ExecuteMethod<int>("WriteLine");
    });

}).Compile();
((Action)newMethod)(); 

//结构体数组遍历输出
Delegate newMethod = EHandler.CreateMethod<ENull>((il) =>
{
    EArray enumArrayModel = EArray.CreateArrayFromRuntimeArray(enumArray);
    ELoop.For(enumArrayModel, (currentElement) =>
    {
        //使用action方式来加载当前元素
        EModel model = EModel.CreateModelFromAction(
            currentElement, 
            typeof(DocumentEnum)
        );
        model.Load();

        //加载之后装箱
        EPacket.Packet(typeof(DocumentEnum));

        //用object参数的方式输出
        method.ExecuteMethod<object>("WriteLine");
    });
}).Compile();
((Action)newMethod)(); 
```

* 条件:
``` csharp
 TestClass t = new TestClass();
 t.Field = 10;
 t.Property = 20;

Delegate showResult = EHandler.CreateMethod<ENull>((il) =>
{
    EMethod method = typeof(Console);

    EVar emit_A = EVar.CreateWithoutTempVar(10);
    EVar emit_B = EVar.CreateVarFromObject(20);
    EModel model = EModel.CreateModelFromObject(t);

    //注意model.DLoad方法，D代表Delay的意思就是延迟加载
    EJudge.If(emit_A == model.DLoad("Field").Model)(() =>
    {
        method.ExecuteMethod<int>("WriteLine", 10);

    }).ElseIf(emit_A > emit_B)(() =>
    {
        method.ExecuteMethod<int>("WriteLine", emit_A);

    }).Else(() =>
    {
        method.ExecuteMethod<int>("WriteLine", emit_B);
    });
}).Compile();
((Action)showResult)();
```

* while循环:
``` csharp
Delegate showResult = EHandler.CreateMethod<ENull>((il) =>
{
    EMethod method = typeof(Console);

    EVar emit_A = EVar.CreateWithoutTempVar(16);
    EVar emit_B = EVar.CreateVarFromObject(20);

    ELoop.While(emit_A < emit_B)(() =>
    {
        method.ExecuteMethod<int>("WriteLine", emit_B);
        emit_B--;
    });


    TestClass t = new TestClass() { Field = 10 };
    //对运行时t对象进行深度复制，并填充到临时变量，以供IL层使用
    EModel model = EModel.CreateModelFromObject(t);

    ELoop.While(model.DLoad("Field").Model < emit_B)(() =>
    {
        //Console.WriteLine(model.field);
        //这里需要传递委托，不传递委托则返回的是model类型而不是int类型
        method.ExecuteMethod<int>(
            "WriteLine", 
            model.DLoad("Field").DelayAction
        );

        //model.Field++
        model.DLoad("Field").Model++;
    });

    ELoop.While(model.DLoad("Property").Model != 25)(() =>
    {
        method.ExecuteMethod<int>(
            "WriteLine", 
            model.DLoad("Property").DelayAction
        );
        model.DLoad("Property").Model++;
    });

}).Compile();
((Action)showResult)();
```

* 自定义类/使用自定义类:
``` csharp
ClassBuilder builder = ClassBuilder.CreateModel("Hello");
builder.CreateDefaultConstructor();
builder.CreateField<string>("Age", FieldAttributes.Public);
builder.CreateProperty<string>("Name");
builder.CreateMethod<ENull>("Show", MethodAttributes.Public, (classModel) =>
{
    classModel.SField("Age", "This is Age.");
    classModel.SProperty("Name", "This is name.");
    classModel.LPropertyValue("Name");
    classModel.ilHandler.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
    classModel.ilHandler.Emit(OpCodes.Ret);
    //下一版酌情对Method创建做优化
});
builder.EndBuilder();

//使用自定义类
Delegate ShowDelegate = EHandler.CreateMethod<ENull>((il) =>
{
    ClassHandler Model = ClassHandler.CreateInstance("Hello");
    Model.EMethod("Show");
    Model.LField("Age");
    Model.ilHandler.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
}).Compile();
((Action)ShowDelegate)();
```

* 补充:

    更多的详细用法请参照源代码中工程例子，对于迭代接口请实现Iiterator接口，并调用Eloop的Foreach方法进行遍历。 
    以下是List的迭代接口实现案例，考虑在下一版中实现对迭代器的支持；

* 迭代器实现:
``` csharp
#region 迭代器属性和方法(不支持)
public int Length
{
    get;
    set;
}
#endregion

#region 迭代器属性和方法(支持)
public LocalBuilder TempEnumerator 
{
    get;
    set;
}
public MethodInfo MoveNext
{
    get;
    set;
}
public MethodInfo Current
{
    get;
    set;
}
public MethodInfo Dispose
{
    get;
    set;
}
public void LoadCurrentElement(LocalBuilder currentBuilder)
{
    ilHandler.Emit(OpCodes.Ldloca, TempEnumerator);
    ilHandler.Emit(OpCodes.Call, Current);
}
public void Initialize()
{
    if (TempEnumerator == null)
    {
        TempEnumerator = ilHandler.DeclareLocal(typeof(List<T>.Enumerator));
        MethodInfo GetEnumerator =TypeHandler.GetMethod(
            "GetEnumerator", 
            new Type[0]
        );
        LoadAddress();
        ilHandler.Emit(OpCodes.Callvirt, GetEnumerator);
        ilHandler.Emit(OpCodes.Stloc, TempEnumerator);
        MoveNext = typeof(List<T>.Enumerator).GetMethod(
            "MoveNext", 
            new Type[0]
        );
        Current = typeof(List<T>.Enumerator).GetProperty("Current").GetGetMethod(true);
        Dispose = typeof(List<T>.Enumerator).GetMethod("Dispose", new Type[0]);
    }
}
#endregion
```

## 愿景

1. 希望大家以平和的心态去对待菜鸟开源库的事情；
2. Json.net 不也是人写出来的吗？Dapper是凭空产生的吗？希望大家发挥自己的创造力，共创辉煌？

## 计划

1. 本库采用MPL2.0开源协议、召集小伙伴继续完善下去，欢迎加入到开发队伍中，QQ群号：573026640；
2. Natasha每年更一次正式版，版本号用去年的年号；

## 吐槽

    对于说我重复造轮子的人我没什么好说的，谁造谁知道；

    对于说我装B的人我想说一下：你猜错了，我想装C，装完C还想装D；

    这个库2016年开始封装，中间推翻了两版，重构十次以上，精神分裂、懵逼N次，怀疑人生1次，求小伙伴们一起参与到建设中来!

    为什么拿Natasha来命名这个库，因为她墓志铭上有这两句话：

    痛苦如此持久，像蜗牛充满耐心地移动；
    快乐如此短暂，像兔子的尾巴掠过秋天的草原；（跟开发过程相符）

## 结尾
    
    我相信，这行没有学习能力是走不远的。

    我也相信，没有创造力是看不清未来的。
    

愿这个库能给您的项目带来方便，给创作带来便利与灵感。 

希望.NET在接下来的发展中能吸引更多的创造者，让.NET世界百花齐放。