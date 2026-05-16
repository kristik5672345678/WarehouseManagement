using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WarehouseManagement.Data.Context;
using WarehouseManagement.Data.Interfaces;
using WarehouseManagement.Data.Models;

namespace WarehouseManagement.Data.Services
{
    public class ServiceOrgs : IService<Organization>
    {
        private ApplicationContext _context {  get; set; }

        public ServiceOrgs(ApplicationContext context)
        {
            if (context != null)
                _context = context;
        }

        public ObservableCollection<Organization> GetAll()
        {
            return _context.Orgs;
        }

        public bool Add(Organization org)
        {
            if (org == null 
                || string.IsNullOrEmpty(org.OrgName) 
                || _context.Orgs.Contains(org))
                return false;

            _context.Orgs.Add(org);
            return true;
        }

        public bool Delete(Organization org)
        {
            if (org == null
                || string.IsNullOrEmpty(org.OrgName)
                || !_context.Orgs.Contains(org))
                return false;

            _context.Orgs.Remove(org);
            return true;
        }

        public bool Edit(Organization org)
        {
            if (org == null
                || string.IsNullOrEmpty(org.OrgName))
                return false;

            int index = _context.Orgs
                .ToList()
                .FindIndex(o => o.Id == org.Id);

            if (index == -1)
                return false;

            _context.Orgs[index] = org;
            return true;
        }

        public ApplicationContext GetContext()
         => _context;
    }
}
