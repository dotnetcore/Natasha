namespace RefAssembly.Compile.Access
{
    public class AccessModelTest
    {
        private string PrivateName = "Private";
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
