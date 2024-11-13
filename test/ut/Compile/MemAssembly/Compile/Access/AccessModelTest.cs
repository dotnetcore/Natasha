namespace MemAssembly.Compile.Access
{
    public class AccessModelTest
    {
        private readonly string PrivateName = "Private";
        private string GetPrivate()
        {
            return PrivateName;
        }
        internal string InternalName = "Internal";
        internal string GetInternal()
        {
            return InternalName;
        }
        public string PublicName = "Public";
        public string GetPublic()
        {
            return PublicName;
        }
    }
}
