using Natasha;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace NatashaUT
{
    [Trait("克隆测试", "")]
    public class DynamicCloneTest
    {

        [Fact(DisplayName = "字段--基元类型以及结构体")]
        public void Normal()
        {
            FieldCloneNormalModel model = new FieldCloneNormalModel
            {
                Age = 1000,
                Name = "ababab",
                Timer = DateTime.Now,
                money = 100000,
                Flag = CloneEnum.A,
                Title = false,
                Id = 100000
            };

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
            FieldCloneArrayModel model = new FieldCloneArrayModel
            {
                Name = new string[10]
            };
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
            FieldCloneClassArrayModel model = new FieldCloneClassArrayModel
            {
                Models = new FieldCloneNormalModel[10]
            };
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
            FieldCloneSubNodeModel model = new FieldCloneSubNodeModel
            {
                Node = new FieldCloneNormalModel() { Age = 1, Name = "111" }
            };

            var newModel = CloneOperator.Clone(model);

            Assert.Equal(model.Node.Name, newModel.Node.Name);
            Assert.Equal(model.Node.Age, newModel.Node.Age);
        }



        [Fact(DisplayName = "字段--类集合")]
        public void ClassCollectionArray()
        {
            FieldCloneClassCollectionModel model = new FieldCloneClassCollectionModel
            {
                Nodes = new List<FieldCloneNormalModel>()
            };
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
            PropCloneNormalModel model = new PropCloneNormalModel
            {
                Age = 1000,
                Name = "ababab",
                Timer = DateTime.Now,
                money = 100000,

                Title = false,
                Id = 100000
            };

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
            PropCloneArrayModel model = new PropCloneArrayModel
            {
                Name = new string[10]
            };
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
            PropCloneClassArrayModel model = new PropCloneClassArrayModel
            {
                Models = new PropCloneNormalModel[10]
            };
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
            PropCloneSubNodeModel model = new PropCloneSubNodeModel
            {
                Node = new PropCloneNormalModel() { Age = 1, Name = "111" }
            };


            var newModel = CloneOperator.Clone(model);


            Assert.Equal(model.Node.Name, newModel.Node.Name);
            Assert.Equal(model.Node.Age, newModel.Node.Age);
        }



        [Fact(DisplayName = "属性--类集合")]
        public void PropClassCollectionTest()
        {
            PropCloneClassCollectionModel model = new PropCloneClassCollectionModel
            {
                Nodes = new List<PropCloneNormalModel>()
            };
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
            CloneCollectionModel model = new CloneCollectionModel
            {
                LANodes = new List<PropCloneNormalModel[]>()
            };
            for (int i = 0; i < 5; i++)
            {
                model.LANodes.Add(new PropCloneNormalModel[10]);
                for (int j = 0; j < 10; j++)
                {
                    model.LANodes[i][j] = new PropCloneNormalModel() { Age = j, Name = j.ToString() };
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
            CloneCollectionModel model = new CloneCollectionModel
            {
                ALNodes = new List<PropCloneNormalModel>[5]
            };
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
            CloneDictModel model = new CloneDictModel
            {
                Dicts = new Dictionary<string, string>()
            };
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
            CloneDictCollectionModel model = new CloneDictCollectionModel
            {
                Dicts = new Dictionary<List<string>, List<FieldCloneNormalModel>>()
            };
            var key1 = new List<string>() { "1" };
            var key2 = new List<string>() { "2" };
            model.Dicts[key1] = new List<FieldCloneNormalModel>(){ new FieldCloneNormalModel
            {
                Age = 1000,
                Name = "ababab",
                Timer = DateTime.Now,
                money = 100000,
                Flag = CloneEnum.A,
                Title = false,
                Id = 100000
            } };
            model.Dicts[key2] = new List<FieldCloneNormalModel>(){ new FieldCloneNormalModel
            {
                Age = 1000,
                Name = "ababab1",
                Timer = DateTime.Now,
                money = 100000,
                Flag = CloneEnum.B,
                Title = true,
                Id = 0
            } };
            var newModel = CloneOperator.Clone(model);
           
            int i = 0;
            foreach (var item in newModel.Dicts)
            {
                if (i == 0)
                {
                    key1.Add("1b");
                    Assert.NotEqual(item.Key, key1);
                    Assert.NotEqual(model.Dicts[key1][0], item.Value[0]);
                    Assert.Equal(model.Dicts[key1][0].Name, item.Value[0].Name);
                }
                else
                {
                    Assert.Equal(item.Key, key2);
                    Assert.NotEqual(model.Dicts[key2][0], item.Value[0]);
                    Assert.Equal(model.Dicts[key2][0].Name, item.Value[0].Name);
                }
                i += 1;
            }
            Assert.NotEqual(model.Dicts, newModel.Dicts);
        }


        [Fact(DisplayName = "字典数组")]
        public void CloneDictionaryArrayTest()
        {
            CloneDictArrayModel model = new CloneDictArrayModel
            {
                Dicts = new Dictionary<string, FieldCloneNormalModel[]>[5]
            };
            for (int i = 0; i < 5; i++)
            {
                model.Dicts[i] = new Dictionary<string, FieldCloneNormalModel[]>();
                for (int j = 0; j < 5; j++)
                {
                    model.Dicts[i][j.ToString()] = new FieldCloneNormalModel[5];
                    for (int z = 0; z < 5; z++)
                    {
                        model.Dicts[i][j.ToString()][z] = new FieldCloneNormalModel
                        {
                            Age = 1000,
                            Name = "ababab1",
                            Timer = DateTime.Now,
                            money = 100000,
                            Flag = CloneEnum.B,
                            Title = true,
                            Id = 0
                        };
                    }
                }
            }

            var newModel = CloneOperator.Clone(model);
            for (int i = 0; i < 5; i++)
            {
                Assert.NotEqual(model.Dicts[i], newModel.Dicts[i]);
                Assert.Equal(model.Dicts[i].Count, newModel.Dicts[i].Count);
                for (int j = 0; j < 5; j++)
                {
                    Assert.NotEqual(model.Dicts[i][j.ToString()], newModel.Dicts[i][j.ToString()]);
                    Assert.Equal(model.Dicts[i][j.ToString()].Length, newModel.Dicts[i][j.ToString()].Length);
                    for (int z = 0; z < 5; z++)
                    {
                        Assert.NotEqual(model.Dicts[i][j.ToString()][z], newModel.Dicts[i][j.ToString()][z]);
                        Assert.Equal(model.Dicts[i][j.ToString()][z].Name, newModel.Dicts[i][j.ToString()][z].Name);
                        Assert.Equal(model.Dicts[i][j.ToString()][z].Age, newModel.Dicts[i][j.ToString()][z].Age);
                        Assert.Equal(model.Dicts[i][j.ToString()][z].Flag, newModel.Dicts[i][j.ToString()][z].Flag);
                        Assert.Equal(model.Dicts[i][j.ToString()][z].Id, newModel.Dicts[i][j.ToString()][z].Id);
                        Assert.Equal(model.Dicts[i][j.ToString()][z].Timer, newModel.Dicts[i][j.ToString()][z].Timer);
                        Assert.Equal(model.Dicts[i][j.ToString()][z].Title, newModel.Dicts[i][j.ToString()][z].Title);
                    }
                }
            }
        }


        [Fact(DisplayName = "C#链表测试")]
        public void CloneLinkTest()
        {
            FieldLinkModel model = new FieldLinkModel();
            model.Nodes.AddFirst(new FieldLinkModel()
            {
                Name = "1",
                Age = 1
            });
            model.Nodes.AddLast(new FieldLinkModel()
            {
                Name = "2",
                Age = 2
            });


            var newModel = CloneOperator.Clone(model);
            newModel.Nodes  = CloneOperator.Clone(model.Nodes);
            Assert.NotEqual(model.Nodes.First, newModel.Nodes.First);
            Assert.Equal(model.Nodes.First.Value.Name, newModel.Nodes.First.Value.Name);

            Assert.NotEqual(model.Nodes.First.Next, newModel.Nodes.First.Next);
            Assert.Equal(model.Nodes.First.Next.Value.Name, newModel.Nodes.First.Next.Value.Name);

        }

        [Fact(DisplayName = "C#链表数组测试")]
        public void CloneLinkArrayTest()
        {
            FieldLinkArrayModel model = new FieldLinkArrayModel
            {
                Nodes = new LinkedList<FieldLinkArrayModel>[5]
            };


            for (int i = 0; i < 5; i++)
            {
                model.Nodes[i] = new LinkedList<FieldLinkArrayModel>();
                model.Nodes[i].AddLast(new FieldLinkArrayModel()
                {
                    Name = i.ToString(),
                    Age = i
                });
                model.Nodes[i].AddLast(new FieldLinkArrayModel()
                {
                    Name = (i+1).ToString(),
                    Age = i+1
                });
            }


            var newModel = CloneOperator.Clone(model);
            newModel.Nodes = CloneOperator.Clone(model.Nodes);

            Assert.NotEqual(model.Nodes, newModel.Nodes);
            Assert.Equal(model.Nodes.Length, newModel.Nodes.Length);

            for (int i = 0; i < 5; i++)
            {
                Assert.NotEqual(model.Nodes[i], newModel.Nodes[i]);
                Assert.NotEqual(model.Nodes[i].First, newModel.Nodes[i].First);
                Assert.Equal(model.Nodes[i].First.Value.Name, newModel.Nodes[i].First.Value.Name);
                Assert.NotEqual(model.Nodes[i].First.Next, newModel.Nodes[i].First.Next);
                Assert.Equal(model.Nodes[i].First.Next.Value.Name, newModel.Nodes[i].First.Next.Value.Name);
            }
        }


        [Fact(DisplayName = "自实现链表测试")]
        public void CloneSelfTest()
        {
            FieldSelfLinkModel model = new FieldSelfLinkModel()
            {
                Name = "1",
                Age = 1
            };
            model.Next = new FieldSelfLinkModel()
            {
                Name = "2",
                Age = 2
            };


            var newModel = CloneOperator.Clone(model);
            Assert.NotEqual(model.Next, newModel.Next);
            Assert.Equal(model.Next.Name, newModel.Next.Name);
        }


        [Fact(DisplayName = "自实现链表数组测试")]
        public void CloneArraySelfTest()
        {
            FieldSelfLinkArrayModel model = new FieldSelfLinkArrayModel()
            {
                Name = "1",
                Age = 1
            };


            model.Next = new FieldSelfLinkArrayModel[5];
            for (int i = 0; i < 5; i++)
            {
                model.Next[i] = new FieldSelfLinkArrayModel()
                {
                    Name = i.ToString(),
                    Age = i
                };
            }
            

            var newModel = CloneOperator.Clone(model);
            Assert.NotEqual(model.Next, newModel.Next);
            for (int i = 0; i < 5; i++)
            {
                Assert.NotEqual(model.Next[i], newModel.Next[i]);
                Assert.Equal(model.Next[i].Name, newModel.Next[i].Name);
            }
           
        }
    }
}
