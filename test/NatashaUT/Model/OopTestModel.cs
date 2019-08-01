using System.Threading.Tasks;

namespace NatashaUT.Model
{
    class OopTestModel
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

    }
}
