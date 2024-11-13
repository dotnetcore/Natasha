using System;

namespace NatashaUT.Model
{
    public class RemoteTestModel
    {
        public void Say()
        {
            Console.WriteLine("hello world");
        }

        public string HelloString(string str1,string str2)
        {
            return str1 + str2;
        }
        public int HelloInt(int str1, int str2)
        {
            return str1 + str2;
        }
    }
}
