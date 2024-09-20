public class CompilePrepareBase
{
    static CompilePrepareBase()
    {
        NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
        NatashaManagement.Preheating(false, false);
    }

}

