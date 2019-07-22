using System;
using System.Collections.Generic;
using System.Text;

namespace Core22
{
    public delegate string GetterDelegate(int value);
    class TestA
    {
    }

    public static class TestStatic
    {

    }

    public abstract class TestAbstract
    {
        public string Name;
        public int Age;
        public abstract int GetAge();
        public abstract string GetName();
    }
}
