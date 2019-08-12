# Convenient Extension  
 <br/>  
 

#### Using Class-Extensions in Natasha:  

```C#

Example:          
         
         
        typeof(Dictionary<string,List<int>>[]).GetDevelopName();
        //result:  "Dictionary<String,List<Int32>>[]"
        
              
        typeof(Dictionary<string,List<int>>[]).GetAvailableName();
        //result:  "Dictionary_String_List_Int32____"
        
           
        typeof(Dictionary<string,List<int>>).GetAllGenericTypes(); 
        //result:  [string,list<>,int]
        
        
        typeof(Dictionary<string,List<int>>).IsImplementFrom<IDictionary>(); 
        //result: true
        
        
        typeof(Dictionary<string,List<int>>).IsOnceType();         
        //result: false
        
        
        typeof(List<>).With(typeof(int));                          
        //result: List<int>

```
<br/>
<br/>    

#### Using Method-Extensions in Natasha:  

```C#

Example:  

        Using : Natasha.MethodExtension; 
        public delegate int AddOne(int value);
        
        
        var action = "return value + 1;".Create<AddOne>();
        var result = action(9);
        //result : 10
        
        
        var action = typeof(AddOne).Create("return value + 1;");
        var result = action(9);
        //result : 10
        
        
        //Use method extension to quickly and dynamically build delegates
         @"string result = str1 +"" ""+ str2;
           Console.WriteLine(result);
           return result;".FastOperator()
               .Param<string>("str1")
               .Param<string>("str2")
               .Return<string>()
               .Complie<Func<string, string, string>>()
```
<br/>
<br/>    
 
 #### Using Clone-Extensions in Natasha:  

```C#

Example:  

        Using : Natasha.CloneExtension; 
        var instance = new ClassA();
        var result = instance.Clone();
```
<br/>
<br/>    
 
  #### Using Snapshot-Extensions in Natasha:  

```C#

Example:  

        Using : Natasha.SnapshotExtension; 
        var instance = new ClassA();
        
        instance.MakeSnapshot();
        
        // ********
        //  do sth
        // ********
        
        var result = instance.Compare();
```

<br/>
