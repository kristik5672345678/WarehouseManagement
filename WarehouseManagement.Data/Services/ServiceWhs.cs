using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WarehouseManagement.Data.Context;
using WarehouseManagement.Data.Interfaces;
using WarehouseManagement.Data.Models;

namespace WarehouseManagement.Data.Services
{
    public class ServiceWhs : IService<Warehouse>
    {
        private ApplicationContext _context;
        private Organization _org;

        public ServiceWhs(ApplicationContext context, Organization org)
        {
            if (context != null && org != null)
            {
                _context = context;
                if (_context.Orgs.Contains(org))
                    _org = org;
            }
        }

        public ObservableCollection<Warehouse> GetAll()
        {
            return new ObservableCollection<Warehouse>(_context.Whs
                .Where(x => x.OrgId == _org.Id)
                .ToList());
        }

        public bool Add(Warehouse whs)
        {
            if (whs == null
                || string.IsNullOrEmpty(whs.WhName)
                || _context.Whs.Contains(whs)
                || _org.Id != whs.OrgId)
                return false;

            _context.Whs.Add(whs);
            return true;
        }

        public bool Delete(Warehouse whs)
        {
            if (whs == null || string.IsNullOrEmpty(whs.WhName) || _org.Id != whs.OrgId)
                return false;

            Warehouse? target = _context.Whs
                        .FirstOrDefault(w => w.Id == whs.Id);

            if (target == null)
                return false;

            _context.Whs.Remove(target);
            return true;
        }

        public bool Edit(Warehouse whs)
        {
            if (whs == null || string.IsNullOrEmpty(whs.WhName) || _org.Id != whs.OrgId)
                return false;

            int index = _context.Whs
                .ToList()
                .FindIndex(w => w.Id == whs.Id);

            if (index == -1)
                return false;

            _context.Whs[index] = whs;
            return true;
        }

        public ApplicationContext GetContext()
            => _context;

        public Organization GetOrganization()
            => _org;
    }
}
