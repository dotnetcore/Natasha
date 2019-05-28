using Natasha;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace NatashaUT
{
    public class DynamicCloneTest
    {
        [Fact(DisplayName = "字段--基元类型以及结构体克隆测试")]
        public void Normal()
        {
            FieldCloneNormalModel model = new FieldCloneNormalModel();
            model.Age = 1000;
            model.Name = "ababab";
            model.Timer = DateTime.Now;
            model.money = 100000;
            model.Flag = CloneEnum.A;
            model.Title = false;
            model.Id = 100000;
            

            CloneBuilder<FieldCloneNormalModel>.CreateCloneDelegate();

            var newModel = DeepClone<FieldCloneNormalModel>.CloneDelegate(model);
            Assert.Equal(model.Id, newModel.Id);
            Assert.Equal(model.Title, newModel.Title);
            Assert.Equal(model.money, newModel.money);
            Assert.Equal(model.Timer, newModel.Timer);
            Assert.Equal(model.Age, newModel.Age);
            Assert.Equal(model.Name, newModel.Name);
        }

        [Fact(DisplayName = "字段--时间以及非类数组克隆测试")]
        public void NotClassArray()
        {
            FieldCloneArrayModel model = new FieldCloneArrayModel();
            
            model.Name = new string[10];
            for (int i = 0; i < 10; i++)
            {
                model.Name[i] = i.ToString();
            }

            CloneBuilder<FieldCloneArrayModel>.CreateCloneDelegate();

            var newModel = DeepClone<FieldCloneArrayModel>.CloneDelegate(model);

           
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Name[i], newModel.Name[i]);
            }

        }
        [Fact(DisplayName = "字段--类数组克隆测试")]
        public void ClassArray()
        {
            FieldCloneClassArrayModel model = new FieldCloneClassArrayModel();
            
            model.Models = new FieldCloneNormalModel[10];
            for (int i = 0; i < 10; i++)
            {
                model.Models[i] = new FieldCloneNormalModel() { Age = i, Name = i.ToString() };
            }

            CloneBuilder<FieldCloneClassArrayModel>.CreateCloneDelegate();

            var newModel = DeepClone<FieldCloneClassArrayModel>.CloneDelegate(model);

            
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Models[i].Name, newModel.Models[i].Name);
                Assert.Equal(model.Models[i].Age, newModel.Models[i].Age);
            }

        }

        [Fact(DisplayName = "字段--子节点克隆测试")]
        public void SubClassArray()
        {
            FieldCloneSubNodeModel model = new FieldCloneSubNodeModel();
            
            model.Node = new FieldCloneNormalModel() { Age = 1, Name = "111" };

            CloneBuilder<FieldCloneSubNodeModel>.CreateCloneDelegate();
            var newModel = DeepClone<FieldCloneSubNodeModel>.CloneDelegate(model);

            
            Assert.Equal(model.Node.Name, newModel.Node.Name);
            Assert.Equal(model.Node.Age, newModel.Node.Age);
        }


        [Fact(DisplayName = "字段--类集合克隆测试")]
        public void ClassCollectionArray()
        {
            FieldCloneClassCollectionModel model = new FieldCloneClassCollectionModel();
          
            model.Nodes = new List<FieldCloneNormalModel>();
            for (int i = 0; i < 10; i++)
            {
                model.Nodes.Add(new FieldCloneNormalModel() { Age = i, Name = i.ToString() });
            }

            CloneBuilder<FieldCloneClassCollectionModel>.CreateCloneDelegate();
            var newModel = DeepClone<FieldCloneClassCollectionModel>.CloneDelegate(model);
           
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Nodes[i].Name, newModel.Nodes[i].Name);
                Assert.Equal(model.Nodes[i].Age, newModel.Nodes[i].Age);
            }

        }

        [Fact(DisplayName = "属性--基元类型以及结构体克隆测试")]
        public void PropNormal()
        {
            PropCloneNormalModel model = new PropCloneNormalModel();
            model.Age = 1000;
            model.Name = "ababab";
            model.Timer = DateTime.Now;
            model.money = 100000;

            model.Title = false;
            model.Id = 100000;


            CloneBuilder<PropCloneNormalModel>.CreateCloneDelegate();

            var newModel = DeepClone<PropCloneNormalModel>.CloneDelegate(model);
            Assert.Equal(model.Id, newModel.Id);
            Assert.Equal(model.Title, newModel.Title);
            Assert.Equal(model.money, newModel.money);
            Assert.Equal(model.Timer, newModel.Timer);
            Assert.Equal(model.Age, newModel.Age);
            Assert.Equal(model.Name, newModel.Name);
        }

        [Fact(DisplayName = "属性--时间以及非类数组克隆测试")]
        public void PropNotClassArray()
        {
            PropCloneArrayModel model = new PropCloneArrayModel();

            model.Name = new string[10];
            for (int i = 0; i < 10; i++)
            {
                model.Name[i] = i.ToString();
            }

            CloneBuilder<PropCloneArrayModel>.CreateCloneDelegate();

            var newModel = DeepClone<PropCloneArrayModel>.CloneDelegate(model);


            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Name[i], newModel.Name[i]);
            }

        }
        [Fact(DisplayName = "属性--类数组克隆测试")]
        public void PropClassArray()
        {
            PropCloneClassArrayModel model = new PropCloneClassArrayModel();

            model.Models = new PropCloneNormalModel[10];
            for (int i = 0; i < 10; i++)
            {
                model.Models[i] = new PropCloneNormalModel() { Age = i, Name = i.ToString() };
            }

            CloneBuilder<PropCloneClassArrayModel>.CreateCloneDelegate();

            var newModel = DeepClone<PropCloneClassArrayModel>.CloneDelegate(model);


            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Models[i].Name, newModel.Models[i].Name);
                Assert.Equal(model.Models[i].Age, newModel.Models[i].Age);
            }

        }

        [Fact(DisplayName = "属性--子节点克隆测试")]
        public void PropSubClassArray()
        {
            PropCloneSubNodeModel model = new PropCloneSubNodeModel();

            model.Node = new PropCloneNormalModel() { Age = 1, Name = "111" };

            CloneBuilder<PropCloneSubNodeModel>.CreateCloneDelegate();
            var newModel = DeepClone<PropCloneSubNodeModel>.CloneDelegate(model);


            Assert.Equal(model.Node.Name, newModel.Node.Name);
            Assert.Equal(model.Node.Age, newModel.Node.Age);
        }


        [Fact(DisplayName = "属性--类集合克隆测试")]
        public void PropClassCollectionArray()
        {
            PropCloneClassCollectionModel model = new PropCloneClassCollectionModel();

            model.Nodes = new List<PropCloneNormalModel>();
            for (int i = 0; i < 10; i++)
            {
                model.Nodes.Add(new PropCloneNormalModel() { Age = i, Name = i.ToString() });
            }

            CloneBuilder<PropCloneClassCollectionModel>.CreateCloneDelegate();
            var newModel = DeepClone<PropCloneClassCollectionModel>.CloneDelegate(model);

            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Nodes[i].Name, newModel.Nodes[i].Name);
                Assert.Equal(model.Nodes[i].Age, newModel.Nodes[i].Age);
            }

        }
    }
}
