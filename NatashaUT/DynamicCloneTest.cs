using Natasha;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace NatashaUT
{
    [Trait("克隆测试","")]
    public class DynamicCloneTest
    {

        [Fact(DisplayName = "字段--基元类型以及结构体")]
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
            
            var newModel = CloneOperator.Clone(model);
            Assert.Equal(model.Id, newModel.Id);
            Assert.Equal(model.Title, newModel.Title);
            Assert.Equal(model.money, newModel.money);
            Assert.Equal(model.Timer, newModel.Timer);
            Assert.Equal(model.Age, newModel.Age);
            Assert.Equal(model.Name, newModel.Name);
        }

        [Fact(DisplayName = "字段--时间以及非类数组")]
        public void NotClassArray()
        {
            FieldCloneArrayModel model = new FieldCloneArrayModel();
            
            model.Name = new string[10];
            for (int i = 0; i < 10; i++)
            {
                model.Name[i] = i.ToString();
            }

            var newModel = CloneOperator.Clone(model);

           
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Name[i], newModel.Name[i]);
            }
        }



        [Fact(DisplayName = "字段--类数组")]
        public void ClassArray()
        {
            FieldCloneClassArrayModel model = new FieldCloneClassArrayModel();
            
            model.Models = new FieldCloneNormalModel[10];
            for (int i = 0; i < 10; i++)
            {
                model.Models[i] = new FieldCloneNormalModel() { Age = i, Name = i.ToString() };
            }

            var newModel = CloneOperator.Clone(model);

            
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Models[i].Name, newModel.Models[i].Name);
                Assert.Equal(model.Models[i].Age, newModel.Models[i].Age);
            }
        }



        [Fact(DisplayName = "字段--子节点")]
        public void SubClassArray()
        {
            FieldCloneSubNodeModel model = new FieldCloneSubNodeModel();
            
            model.Node = new FieldCloneNormalModel() { Age = 1, Name = "111" };

            var newModel = CloneOperator.Clone(model);

            Assert.Equal(model.Node.Name, newModel.Node.Name);
            Assert.Equal(model.Node.Age, newModel.Node.Age);
        }



        [Fact(DisplayName = "字段--类集合")]
        public void ClassCollectionArray()
        {
            FieldCloneClassCollectionModel model = new FieldCloneClassCollectionModel();
          
            model.Nodes = new List<FieldCloneNormalModel>();
            for (int i = 0; i < 10; i++)
            {
                model.Nodes.Add(new FieldCloneNormalModel() { Age = i, Name = i.ToString() });
            }

            var newModel = CloneOperator.Clone(model);
           
            for (int i = 0; i < 10; i++)
            {
                Assert.NotEqual(model.Nodes, newModel.Nodes);
                Assert.Equal(model.Nodes[i].Name, newModel.Nodes[i].Name);
                Assert.Equal(model.Nodes[i].Age, newModel.Nodes[i].Age);
            }
        }



        [Fact(DisplayName = "属性--基元类型以及结构体")]
        public void PropNormal()
        {
            PropCloneNormalModel model = new PropCloneNormalModel();
            model.Age = 1000;
            model.Name = "ababab";
            model.Timer = DateTime.Now;
            model.money = 100000;

            model.Title = false;
            model.Id = 100000;

            var newModel = CloneOperator.Clone(model);
            Assert.Equal(model.Id, newModel.Id);
            Assert.Equal(model.Title, newModel.Title);
            Assert.Equal(model.money, newModel.money);
            Assert.Equal(model.Timer, newModel.Timer);
            Assert.Equal(model.Age, newModel.Age);
            Assert.Equal(model.Name, newModel.Name);
        }



        [Fact(DisplayName = "属性--时间以及非类数组")]
        public void PropNotClassArray()
        {
            PropCloneArrayModel model = new PropCloneArrayModel();

            model.Name = new string[10];
            for (int i = 0; i < 10; i++)
            {
                model.Name[i] = i.ToString();
            }

            var newModel = CloneOperator.Clone(model);


            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Name[i], newModel.Name[i]);
            }
        }



        [Fact(DisplayName = "属性--类数组")]
        public void PropClassArray()
        {
            PropCloneClassArrayModel model = new PropCloneClassArrayModel();

            model.Models = new PropCloneNormalModel[10];
            for (int i = 0; i < 10; i++)
            {
                model.Models[i] = new PropCloneNormalModel() { Age = i, Name = i.ToString() };
            }

            var newModel = CloneOperator.Clone(model);


            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(model.Models[i].Name, newModel.Models[i].Name);
                Assert.Equal(model.Models[i].Age, newModel.Models[i].Age);
            }

        }



        [Fact(DisplayName = "属性--子节点")]
        public void PropSubClassArray()
        {
            PropCloneSubNodeModel model = new PropCloneSubNodeModel();

            model.Node = new PropCloneNormalModel() { Age = 1, Name = "111" };


            var newModel = CloneOperator.Clone(model);


            Assert.Equal(model.Node.Name, newModel.Node.Name);
            Assert.Equal(model.Node.Age, newModel.Node.Age);
        }



        [Fact(DisplayName = "属性--类集合")]
        public void PropClassCollectionTest()
        {
            PropCloneClassCollectionModel model = new PropCloneClassCollectionModel();

            model.Nodes = new List<PropCloneNormalModel>();
            for (int i = 0; i < 10; i++)
            {
                model.Nodes.Add(new PropCloneNormalModel() { Age = i, Name = i.ToString() });
            }

            var newModel = CloneOperator.Clone(model);

            for (int i = 0; i < 10; i++)
            {
                Assert.NotEqual(model.Nodes, newModel.Nodes);
                Assert.Equal(model.Nodes[i].Name, newModel.Nodes[i].Name);
                Assert.Equal(model.Nodes[i].Age, newModel.Nodes[i].Age);
            }

        }



        [Fact(DisplayName = "类集合嵌套集合")]
        public void PropClassCollectionArray1()
        {
            CloneCollectionModel model = new CloneCollectionModel();
            var INodes = new List<List<PropCloneNormalModel>>();
            for (int i = 0; i < 5; i++)
            {
                INodes.Add(new List<PropCloneNormalModel>());
                for (int j = 0; j < 10; j++)
                {
                    INodes[i].Add(new PropCloneNormalModel() { Age = j, Name = j.ToString() });
                }
            }
            model.LLNodes = INodes;
            var newModel = CloneOperator.Clone(model);

            Assert.NotEqual(model.LLNodes, newModel.LLNodes);
            var oldNodes = new List<List<PropCloneNormalModel>>(model.LLNodes);
            var newNodes = new List<List<PropCloneNormalModel>>(newModel.LLNodes);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Assert.NotEqual(oldNodes[i], newNodes[i]);
                    Assert.Equal(oldNodes[i][j].Name, newNodes[i][j].Name);
                    Assert.Equal(oldNodes[i][j].Age, newNodes[i][j].Age);
                }
            }
        }



        [Fact(DisplayName = "类集合嵌套数组")]
        public void PropClassCollectionArray2()
        {
            CloneCollectionModel model = new CloneCollectionModel();

            model.LANodes = new List<PropCloneNormalModel[]>();
            for (int i = 0; i < 5; i++)
            {
                model.LANodes.Add(new PropCloneNormalModel[10]);
                for (int j = 0; j < 10; j++)
                {
                    model.LANodes[i][j]=new PropCloneNormalModel() { Age = j, Name = j.ToString() };
                }
            }


            var newModel = CloneOperator.Clone(model);

            for (int i = 0; i < 5; i++)
            {
                Assert.NotEqual(model.LANodes, newModel.LANodes);
                for (int j = 0; j < 10; j++)
                {
                    Assert.NotEqual(model.LANodes[i], newModel.LANodes[i]);
                    Assert.Equal(model.LANodes[i][j].Name, newModel.LANodes[i][j].Name);
                    Assert.Equal(model.LANodes[i][j].Age, newModel.LANodes[i][j].Age);
                }
            }
        }



        [Fact(DisplayName = "类数组嵌套集合")]
        public void PropClassCollectionArray3()
        {
            CloneCollectionModel model = new CloneCollectionModel();

            model.ALNodes = new List<PropCloneNormalModel>[5];
            for (int i = 0; i < 5; i++)
            {
                model.ALNodes[i] = new List<PropCloneNormalModel>();
                for (int j = 0; j < 10; j++)
                {
                    model.ALNodes[i].Add(new PropCloneNormalModel() { Age = j, Name = j.ToString() });
                }
            }

            var newModel = CloneOperator.Clone(model);

            for (int i = 0; i < 5; i++)
            {
                Assert.NotEqual(model.ALNodes, newModel.ALNodes);
                for (int j = 0; j < 10; j++)
                {
                    Assert.NotEqual(model.ALNodes[i], newModel.ALNodes[i]);
                    Assert.Equal(model.ALNodes[i][j].Name, newModel.ALNodes[i][j].Name);
                    Assert.Equal(model.ALNodes[i][j].Age, newModel.ALNodes[i][j].Age);
                }
            }
        }

        [Fact(DisplayName = "字段字典")]
        public void CloneDictionaryTest()
        {
            CloneDictModel model = new CloneDictModel();
            model.Dicts = new Dictionary<string, string>();
            model.Dicts["1"] = "2";
            model.Dicts["2"] = "3";
            var newModel = CloneOperator.Clone(model);
            foreach (var item in newModel.Dicts)
            {
                Assert.Equal(model.Dicts[item.Key], item.Value);
            }
            model.Dicts["1"] = "4";
            Assert.NotEqual(model.Dicts, newModel.Dicts);
        }


        [Fact(DisplayName = "字典集合")]
        public void CloneDictionaryCollectionTest()
        {

        }


        [Fact(DisplayName = "字典数组")]
        public void CloneDictionaryArrayTest()
        {

        }
    }
}
