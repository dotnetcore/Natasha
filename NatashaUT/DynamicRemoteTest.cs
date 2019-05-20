using Natasha.Remote;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT
{
    public class DynamicRemoteTest
    {
        [Fact(DisplayName = "远程调用--委托")]
        public void RemoteDelegateTest()
        {
            RemoteWritter.Serialization = "RemoteTestModel.Serialization";
            RemoteWritter.Deserialization = "RemoteTestModel.Deserialization";
            RemoteWritter.ComplieToRemote<RemoteTestModel>();
            RemoteParameters parameters = new RemoteParameters();
            parameters.TypeName = "RemoteTestModel";
            parameters.MethodName = "Hello";
            Assert.NotNull(RemoteReader.GetFunc(parameters));

        }
        [Fact(DisplayName = "远程调用--结果")]
        public void RemoteInvokeTest()
        {
            RemoteWritter.Serialization = "RemoteTestModel.Serialization";
            RemoteWritter.Deserialization = "RemoteTestModel.Deserialization";
            RemoteWritter.ComplieToRemote<RemoteTestModel>();
            RemoteParameters parameters = new RemoteParameters();
            parameters.TypeName = "RemoteTestModel";
            parameters.MethodName = "Hello";
            parameters["str1"] = "hello ";
            parameters["str2"] = "world!";
            string result = RemoteReader.Invoke(parameters);
            Assert.NotNull(RemoteReader.GetFunc(parameters));
            Assert.Equal("hello world!", result);
        }
    }
}
