namespace HotReloadPlugin
{
    public class A
    {
        public int Code = 3;
        [CLSCompliant(false)]
        public void Show() { Console.WriteLine(Code); }
        [CLSCompliant(false)]
        public void Show2(int i) { Console.WriteLine(Code+i); }
    }
}
