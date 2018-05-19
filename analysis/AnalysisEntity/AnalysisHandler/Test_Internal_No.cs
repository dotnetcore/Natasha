namespace AnalysisEntity
{
    public class Test_Internal_No : TestBase
    {
        public static void Test()
        {
            Show<Internal_InternalEntity>();
            Show<Internal_PrivateEntity>();
            Show<Internal_ProtectedEntity>();
            Show<Internal_ProtectedInternalEntity>();
            Show<Internal_PublicEntity>();
        }
    }
}
