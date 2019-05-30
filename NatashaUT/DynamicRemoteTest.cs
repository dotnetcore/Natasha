using Natasha.Remote;
using NatashaUT.Model;
using Newtonsoft.Json;
using Xunit;

namespace NatashaUT
{
    public class DynamicRemoteTest
    {



        [Fact(DisplayName = "远程调用--客户端参数")]
        public void RemoteParametersTest()
        {
            var parameter = new ParametersMaker<RemoteTestModel>();
            var temp = parameter["HelloString"].Params("hello ", "world!");
            Assert.Equal("RemoteTestModel", temp.TypeName);
            Assert.Equal("HelloString", temp.MethodName);
            Assert.Equal("\"hello \"", temp["str1"]);
            Assert.Equal("\"world!\"", temp["str2"]);
        }



        [Fact(DisplayName = "远程调用--服务端委托")]
        public void RemoteDelegateTest()
        {
            RemoteWritter.ComplieToRemote<RemoteTestModel>();
            RemoteParameters parameters = new RemoteParameters();
            parameters.TypeName = "RemoteTestModel";
            parameters.MethodName = "HelloString";
            Assert.NotNull(RemoteReader.GetFunc(parameters));

        }



        [Fact(DisplayName = "远程调用--服务端结果")]
        public void RemoteInvokeTest()
        {
            RemoteWritter.ComplieToRemote<RemoteTestModel>();


            RemoteParameters parameters = new RemoteParameters();
            parameters.TypeName = "RemoteTestModel";
            parameters.MethodName = "HelloString";
            parameters["str1"] = JsonConvert.SerializeObject("hello ");
            parameters["str2"] = JsonConvert.SerializeObject("world!");


            string result = RemoteReader.Invoke(parameters);
            Assert.Equal("\"hello world!\"", result);


            var parameter = new ParametersMaker<RemoteTestModel>();
            var temp = parameter["HelloString"].Params("hello ", "world!");

            result = JsonConvert.DeserializeObject<string>(RemoteReader.Invoke(temp));
            Assert.Equal("hello world!", result);


            parameter = new ParametersMaker<RemoteTestModel>();
            temp = parameter["HelloInt"].Params(10, 100);

            int intResult = JsonConvert.DeserializeObject<int>(RemoteReader.Invoke(temp));
            Assert.Equal(110, intResult);
        }
    }
}
