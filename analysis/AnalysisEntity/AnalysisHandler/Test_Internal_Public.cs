using static AnalysisEntity.Internal_Public_InternalEntity;

namespace AnalysisEntity.AnalysisHandler
{
    public class Test_Internal_Public:TestBase
    {
        public static void Test()
        {
            Show<Nested_Internal_Public_InternalEntity>();
            Show<Internal_PrivateEntity>();
            Show<Internal_ProtectedEntity>();
            Show<Internal_ProtectedInternalEntity>();
            Show<Internal_PublicEntity>();
        }
        
    }
}
