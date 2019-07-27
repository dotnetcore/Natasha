namespace NatashaUT.Model
{
    public class ReverserTestModel
    {
        public void Test1(in Rsm<GRsm> rsm) { }
        public void Test2(out Rsm<Rsm<GRsm>[]> rsm) { rsm = new Rsm<Rsm<GRsm>[]>(); }
        public void Test3(ref Rsm<Rsm<GRsm>[]>[] rsm) { }
    }

    public class Rsm<T> {

    }

    public class GRsm
    {

    }
}
