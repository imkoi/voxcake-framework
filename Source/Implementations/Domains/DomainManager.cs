using System.Collections.Generic;

namespace VoxCake.Framework
{
    public class DomainManager : IDomainManager
    {
        private readonly IContext _context;
        private readonly List<DomainBase> _domains;
        private readonly List<DomainBase> _installedDomains;
        private readonly List<DomainBase> _enabledDomains;
        
        public DomainManager(Context context)
        {
            _context = context;
            _domains = new List<DomainBase>();
            _installedDomains = new List<DomainBase>();
            _enabledDomains = new List<DomainBase>();
        }
        
        public void AddDomain(DomainBase domain)
        {
            _domains.Add(domain);
        }

        public void InstallDomains()
        {
            foreach (var domain in _domains)
            {
                if (!_installedDomains.Contains(domain))
                {
                    domain.OnInstall(_context);

                    _installedDomains.Add(domain);
                }
            }
        }

        public void EnableDomains()
        {
            foreach (var domain in _domains)
            {
                var canBeEnabled = domain.CanBeEnabled(_context) && !_enabledDomains.Contains(domain);

                if (canBeEnabled)
                {
                    domain.OnEnable(_context);
                    
                    _enabledDomains.Add(domain);
                }
            }   
        }
    }
}