using System.Threading.Tasks;

namespace NatashaUT.Model
{
    public class OopTestModel
    {

        public void ReWrite1()
        {

        }
        public async Task<OopTestModel> ReWrite2()
        {
            return this;
        }

        public virtual void ReWrite3(ref int i, string temp)
        {

        }


        public virtual ref int ReWrite4(ref int i,out string temp)
        {
            temp = default;
            return ref i;
        }

        public class InnerClass 
        {

            public string Name;

        }


    }
}
