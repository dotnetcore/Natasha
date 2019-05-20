using Natasha.Remote;
using NatashaUT.Model;
using Xunit;

namespace NatashaUT
{
    public class DynamicRemoteTest
    {
        [Fact(DisplayName = "远程调用--参数")]
        public void RemoteParametersTest()
        {
            var parameter =new ParametersMaker<RemoteTestModel>();
            var temp = parameter["Hello"].Params("hello ","world!");
            RemoteWritter.ComplieToRemote<RemoteTestModel>();
            string result = RemoteReader.Invoke(temp);
            Assert.Equal("hello world!", result);
        }
        [Fact(DisplayName = "远程调用--委托")]
        public void RemoteDelegateTest()
        {
            RemoteWritter.ComplieToRemote<RemoteTestModel>();
            RemoteParameters parameters = new RemoteParameters();
            parameters.TypeName = "RemoteTestModel";
            parameters.MethodName = "Hello";
            Assert.NotNull(RemoteReader.GetFunc(parameters));

        }
        [Fact(DisplayName = "远程调用--结果")]
        public void RemoteInvokeTest()
        {
            RemoteWritter.ComplieToRemote<RemoteTestModel>();
            RemoteParameters parameters = new RemoteParameters();
            parameters.TypeName = "RemoteTestModel";
            parameters.MethodName = "Hello";
            parameters["str1"] = "hello ";
            parameters["str2"] = "world!";
            string result = RemoteReader.Invoke(parameters);
            Assert.Equal("hello world!", result);
        }
    }
}
