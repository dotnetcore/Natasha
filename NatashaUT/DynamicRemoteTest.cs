using Natasha.Remote;
using NatashaUT.Model;
using Newtonsoft.Json;
using Xunit;

namespace NatashaUT
{
    public class DynamicRemoteTest
    {
        [Fact(DisplayName = "远程调用--参数")]
        public void RemoteParametersTest()
        {
            var parameter =new ParametersMaker<RemoteTestModel>();
            var temp = parameter["HelloString"].Params("hello ","world!");
            RemoteWritter.ComplieToRemote<RemoteTestModel>();
            string result = JsonConvert.DeserializeObject<string>(RemoteReader.Invoke(temp));
            Assert.Equal("hello world!", result);

            parameter = new ParametersMaker<RemoteTestModel>();
            temp = parameter["HelloInt"].Params(10, 100);
            int intResult =JsonConvert.DeserializeObject<int>(RemoteReader.Invoke(temp));
            Assert.Equal(110, intResult);
        }
        [Fact(DisplayName = "远程调用--委托")]
        public void RemoteDelegateTest()
        {
            RemoteWritter.ComplieToRemote<RemoteTestModel>();
            RemoteParameters parameters = new RemoteParameters();
            parameters.TypeName = "RemoteTestModel";
            parameters.MethodName = "HelloString";
            Assert.NotNull(RemoteReader.GetFunc(parameters));

        }
        [Fact(DisplayName = "远程调用--结果")]
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
        }
    }
}
