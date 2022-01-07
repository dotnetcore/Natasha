using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceTest50
{
    public record Test
    {
        public string Name { get; set; }

        public Test Create2()
        {
            Name = "a";
            return this;
        }
        public Test Create()
        {
            return this with { Name = "a" };
        }
    }



    public class Father
    {
        public static Father Default;
        static Father()
        {
            Default = default!;
        }

        public Father(string key)
        {
            if (key == "Default")
            {
                Default = this;
            }
        }

    }

    public class Child : Father
    {

        public new static Child Default;
        static Child()
        {
            Default = new Child("Default");
        }


        public Child(string key) : base(key)
        {
            
        }
    }


}
