using Natasha;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace NatashaUT
{
    [Trait("快照测试", "")]
    public class DynamicSnapshotTest
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

            SnapshotOperator.MakeSnapshot(model);
            model.Age = 1001;
            model.Name = "hahaha";
            model.Flag = CloneEnum.B;
            model.Title = true;
            var reuslt = SnapshotOperator.Compare(model);


            Assert.NotEqual(model.Age, (int)(reuslt["Age"].Value));
            Assert.NotEqual(model.Title, (bool)(reuslt["Title"].Value));
            Assert.NotEqual(model.Name, (string)(reuslt["Name"].Value));
            Assert.NotEqual(model.Flag, (CloneEnum)(reuslt["Flag"].Value));


            Assert.Equal(1000, (int)(reuslt["Age"].Value));
            Assert.False((bool)(reuslt["Title"].Value));
            Assert.Equal("ababab", (string)(reuslt["Name"].Value));
            Assert.Equal(CloneEnum.A, (CloneEnum)(reuslt["Flag"].Value));
        }


        [Fact(DisplayName = "字段--非类数组")]
        public void NotClassArray()
        {
            FieldCloneArrayModel model = new FieldCloneArrayModel();

            model.Name = new string[10];
            for (int i = 0; i < 10; i++)
            {
                model.Name[i] = i.ToString();
            }

            SnapshotOperator.MakeSnapshot(model);
            for (int i = 5; i < 10; i++)
            {
                model.Name[i] = (i + 100).ToString();
            }
            var reuslt = SnapshotOperator.Compare(model);
            var value = (HashSet<string>)reuslt["Name"].Value;
            int temp = 5;
            foreach (var item in value)
            {
                Assert.Equal(temp.ToString(), item);
                temp++;
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

            SnapshotOperator.MakeSnapshot(model);
            for (int i = 5; i < 10; i++)
            {
                model.Models[i] = new FieldCloneNormalModel() { Age = i + 100, Name = (i + 100).ToString() };
            }
            var reuslt = SnapshotOperator.Compare(model);
            var value = (HashSet<FieldCloneNormalModel>)reuslt["Models"].Value;
            int temp = 5;
            foreach (var item in value)
            {
                Assert.Equal(temp.ToString(), item.Name);
                Assert.Equal(temp, item.Age);
                temp++;
            }
        }



        [Fact(DisplayName = "字段--子节点")]
        public void SubClassArray()
        {
            FieldCloneSubNodeModel model = new FieldCloneSubNodeModel();

            model.Node = new FieldCloneNormalModel() { Age = 1, Name = "111" };

            SnapshotOperator.MakeSnapshot(model);

            model.Node = new FieldCloneNormalModel() { Age = 2, Name = "222" };

            var result = SnapshotOperator.Compare(model);
            var value = (Dictionary<string, DiffModel>)(result["Node"].Value);
            Assert.NotEqual(model.Node.Name, (string)(value["Name"].Value));
            Assert.NotEqual(model.Node.Age, (int)(value["Age"].Value));
            Assert.Equal("111", (string)(value["Name"].Value));
            Assert.Equal(1, (int)(value["Age"].Value));
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

            SnapshotOperator.MakeSnapshot(model);

            for (int i = 5; i < 10; i++)
            {
                model.Nodes.Add(new FieldCloneNormalModel() { Age = i + 100, Name = (i + 100).ToString() });
            }


            var reuslt = SnapshotOperator.Compare(model);
            var value = (HashSet<FieldCloneNormalModel>)reuslt["Nodes"].Value;
            int temp = 5;

            foreach (var item in value)
            {
                Assert.Equal(temp.ToString(), item.Name);
                Assert.Equal(temp, item.Age);
                temp++;
            }
        }



        [Fact(DisplayName = "属性--基元类型以及结构体")]
        public void PropNormal()
        {
            var tempDate = DateTime.Now;
            PropCloneNormalModel model = new PropCloneNormalModel();
            model.Age = 1000;
            model.Name = "ababab";
            model.Timer = tempDate;
            model.money = 100000;
            model.Title = false;
            model.Id = 100000;
            //生成快照
            SnapshotOperator.MakeSnapshot(model);

            //更改model
            model.Age = 1001;
            model.Name = "hahaha";
            model.Timer = DateTime.Now;
            model.Title = true;

            //对比快照
            var reuslt = SnapshotOperator.Compare(model);


            Assert.NotEqual(model.Age, (int)(reuslt["Age"].Value));
            Assert.NotEqual(model.Title, (bool)(reuslt["Title"].Value));
            Assert.NotEqual(model.Name, (string)(reuslt["Name"].Value));
            Assert.NotEqual(model.Timer, (DateTime)(reuslt["Timer"].Value));

            Assert.Equal(1000, (int)(reuslt["Age"].Value));
            Assert.False((bool)(reuslt["Title"].Value));
            Assert.Equal("ababab", (string)(reuslt["Name"].Value));
            Assert.Equal(tempDate, (DateTime)(reuslt["Timer"].Value));
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

            SnapshotOperator.MakeSnapshot(model);
            for (int i = 5; i < 10; i++)
            {

                model.Name[i] = (i + 100).ToString();
            }
            var reuslt = SnapshotOperator.Compare(model);
            var value = (HashSet<string>)reuslt["Name"].Value;
            int temp = 5;
            foreach (var item in value)
            {
                Assert.Equal(temp.ToString(), item);
                temp++;
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

            SnapshotOperator.MakeSnapshot(model);
            for (int i = 5; i < 10; i++)
            {
                model.Models[i] = new PropCloneNormalModel() { Age = i + 100, Name = (i + 100).ToString() };
            }
            var reuslt = SnapshotOperator.Compare(model);
            var value = (HashSet<PropCloneNormalModel>)reuslt["Models"].Value;
            int temp = 5;
            foreach (var item in value)
            {
                Assert.Equal(temp.ToString(), item.Name);
                Assert.Equal(temp, item.Age);
                temp++;
            }

        }



        [Fact(DisplayName = "属性--子节点")]
        public void PropSubClassArray()
        {
            PropCloneSubNodeModel model = new PropCloneSubNodeModel();

            model.Node = new PropCloneNormalModel() { Age = 1, Name = "111" };

            SnapshotOperator.MakeSnapshot(model);

            model.Node = new PropCloneNormalModel() { Age = 2, Name = "222" };

            var result = SnapshotOperator.Compare(model);
            var value = (Dictionary<string, DiffModel>)(result["Node"].Value);
            Assert.NotEqual(model.Node.Name, (string)(value["Name"].Value));
            Assert.NotEqual(model.Node.Age, (int)(value["Age"].Value));
            Assert.Equal("111", (string)(value["Name"].Value));
            Assert.Equal(1, (int)(value["Age"].Value));
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

            SnapshotOperator.MakeSnapshot(model);

            for (int i = 5; i < 10; i++)
            {
                model.Nodes.Add(new PropCloneNormalModel() { Age = i + 100, Name = (i + 100).ToString() });
            }


            var reuslt = SnapshotOperator.Compare(model);
            var value = (HashSet<PropCloneNormalModel>)reuslt["Nodes"].Value;
            int temp = 5;

            foreach (var item in value)
            {
                Assert.Equal(temp.ToString(), item.Name);
                Assert.Equal(temp, item.Age);
                temp++;
            }
        }



       

        /*

[Fact(DisplayName = "类集合嵌套数组")]
public void PropClassCollectionArray2()
{
PropCloneCollectionModel model = new PropCloneCollectionModel();

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
PropCloneCollectionModel model = new PropCloneCollectionModel();

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
}*/
    }
}
