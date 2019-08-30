namespace NatashaUT.Model
{

    public interface ITest
    {
        int MethodWidthReturnInt();
        string MethodWidthReturnString();
        void MethodWidthParamsRefInt(ref int i);
        string MethodWidthParamsString(string str);
        string MethodWidthParams(int a, string str, out int b);
    }




    public abstract class ATest
    {
        public abstract int MethodWidthReturnInt();
        public abstract string MethodWidthReturnString();
        public abstract void MethodWidthParamsRefInt(ref int i);
        public abstract string MethodWidthParamsString(string str);
        public abstract string MethodWidthParams(int a, string str,out int b);
    }




    public abstract class VTest
    {
        public virtual int MethodWidthReturnInt() { return default; }
        public virtual string MethodWidthReturnString() { return default; }
        public virtual void MethodWidthParamsRefInt(ref int i) { }
        public virtual string MethodWidthParamsString(string str) { return default;  }
        public virtual string MethodWidthParams(int a, string str,out int b) { b = default; return default;  }
    }




    public abstract class NTest
    {
        public int MethodWidthReturnInt() { return default; }
        public string MethodWidthReturnString() { return default; }
        public void MethodWidthParamsRefInt(ref int i) { }
        public string MethodWidthParamsString(string str) { return default; }
        public string MethodWidthParams(int a, string str, out int b) { b = default; return default; }
    }

}
