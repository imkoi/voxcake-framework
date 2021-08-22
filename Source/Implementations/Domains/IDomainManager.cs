namespace VoxCake.Framework
{
    public interface IDomainManager
    {
        void AddDomain(DomainBase domain);
        void InstallDomains();
        void EnableDomains();
    }
}