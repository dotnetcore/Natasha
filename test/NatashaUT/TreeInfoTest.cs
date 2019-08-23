using Natasha.Complier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NatashaUT
{

    [Trait("引擎模块", "语法树解析")]
    public class TreeInfoTest
    {

        [Fact(DisplayName = "语法诊断")]
        public void Test1()
        {
            string str = @"
     public class TestInfo1{
        
        public string Name;

        public List<string> List;
    }
";
            var (_,_,_,errors) = str;
            foreach (var item in errors)
            {
                var aa = item.GetMessage();
            }
            Assert.Equal(0, errors.Count());
            Assert.Empty(errors);
        }
    }
}
