using System;
using System.Collections.Generic;
using System.Text;

namespace NatashaBenchmark.Model
{
    public class CallModel
    {
        public CallModel()
        {
            Age = "World!";
            CreateTime = DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public string Age;

        public DateTime CreateTime;
    }
}
