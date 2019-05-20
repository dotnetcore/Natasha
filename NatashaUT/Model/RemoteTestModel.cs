using System;

namespace NatashaUT.Model
{
    public class RemoteTestModel
    {
        public void Say()
        {
            Console.WriteLine("hello world");
        }

        public string Hello(string str1,string str2)
        {
            return str1 + str2;
        }

        public static T Serialization<T>(T value) {
            return value;
        }
        public static string Deserialization<T>(T value)
        {
            return value.ToString();
        }
    }
}
