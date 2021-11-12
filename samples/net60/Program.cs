// See https://aka.ms/new-console-template for more information
//NatashaInitializer.InitializeAndPreheating();
using net60;
using System.Collections.ObjectModel;
using System.Reflection;
//var type = typeof(Test<>);


//var gType = type.GetGenericArguments()[0];

//if (gType.ContainsGenericParameters)
//{
//    var cType = gType.GetGenericParameterConstraints();
//    foreach (var item in cType)
//    {
//        Console.WriteLine(item.Name);
//        foreach (var attr in item.CustomAttributes)
//        {
//            Console.WriteLine(attr.AttributeType.Name);
//        }
//    }
//}
//var attrs = gType!.CustomAttributes;
//foreach (var attr in attrs)
//{
//    Console.WriteLine(attr.AttributeType.Name);
//}
foreach (var item in typeof(Test2).GetMethods())
{
    ShowParameterInfos(item);

}


static void ShowParameterInfos(MethodInfo method)
{
    var parameterInfos = method.GetParameters();
    Console.WriteLine(method.Name);
    foreach (var item in parameterInfos)
    {
        Console.Write($"{item.ParameterType.Name}{(item.IsNullable() ? "?" : "")}:{item.Name}  ");
    }


    //NullabilityInfoContext context = new();

    //NullabilityInfo arrayInfo = context.Create(,);
    //Console.WriteLine(arrayInfo.ReadState);        // NotNull
    //Console.WriteLine(arrayInfo.Element.State);    // Nullable

    //NullabilityInfo tupleInfo = context.Create(tupleField);
    //Console.WriteLine(tupleInfo.ReadState);                      // NotNull
    //Console.WriteLine(tupleInfo.GenericTypeArguments[0].State); // Nullable
    //Console.WriteLine(tupleInfo.GenericTypeArguments[1].State); // NotNull
    //foreach (var attr in method.CustomAttributes)
    //{
    //    Console.WriteLine($"{attr.AttributeType.Name} :{(attr.ConstructorArguments.Count > 0 ? attr.ConstructorArguments[0].Value : "")}");
    //}
    /*
    var nullArributeData = method.CustomAttributes.FirstOrDefault(item => item.AttributeType.Name == "NullableContextAttribute");
    if (nullArributeData!=null)
    {
        if ((byte)nullArributeData.ConstructorArguments[0].Value! == 2)
        {
            if (method.ReturnType == typeof(void))
            {
                foreach (var item in parameterInfos)
                {
                    Console.Write($"{item.ParameterType.Name}{(IsNotNullType(item) ? "" : "?")}:{item.Name}  ");
                }
            }
            else
            {
                foreach (var item in parameterInfos)
                {

                    Console.Write($"{item.ParameterType.Name}{(IsNullType(item) ? "?" : "")}:{item.Name}  ");
                }

            }
            Console.WriteLine($"Return: {method.ReturnType.Name}?");
        }
        else  //1
        {
            if (method.ReturnType == typeof(void))
            {
                foreach (var item in parameterInfos)
                {
                    Console.Write($"{item.ParameterType.Name}{(IsNullType(item) ? "?" : "")}:{item.Name}  ");
                }
            }
            else
            {
                foreach (var item in parameterInfos)
                {

                    Console.Write($"{item.ParameterType.Name}{(IsNullType(item) ? "?" : "")}:{item.Name}  ");
                }

            }
            Console.WriteLine($"Return: {method.ReturnType.Name}");
        }
       
    }
    else
    {
        if (method.ReturnType == typeof(void))
        {
            foreach (var item in parameterInfos)
            {
                Console.Write($"{item.ParameterType.Name}{(IsNotNullType(item) ? "" : "?")}:{item.Name}  ");
            }
        }
        else
        {
            foreach (var item in parameterInfos)
            {

                Console.Write($"{item.ParameterType.Name}{(IsNotNullType(item) ? "" : "?")}:{item.Name}  ");
            }
            Console.Write($"Return: {method.ReturnType.Name}?");
        }
    }
    */
    Console.WriteLine();
    Console.WriteLine();
}
static bool IsNotNullType(ParameterInfo parameterInfo)
{
    return parameterInfo.CustomAttributes.Any(item => item.AttributeType.Name == "NullableAttribute");
}
static bool IsNullType(ParameterInfo parameterInfo)
{
    return parameterInfo.CustomAttributes.Any(item => item.AttributeType.Name == "NullableAttribute" && (byte)(item.ConstructorArguments[0].Value!) == 2);
}

Console.ReadKey();