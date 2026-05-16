using System.Collections.ObjectModel;
using WarehouseManagement.Data.Context;
using WarehouseManagement.Data.Interfaces;
using WarehouseManagement.Data.Models;

namespace WarehouseManagement.Data.Services
{
    public class ServiceInvoices : IService<Invoice>
    {
        private ApplicationContext _context;
        private Warehouse _whs;

        public ServiceInvoices(ApplicationContext context, Warehouse whs)
        {
            if (context != null && whs != null)
            {
                _context = context;
                _whs = whs;
            }
        }

        public ObservableCollection<Invoice> GetAll()
        {
            return new ObservableCollection<Invoice>(_context.Invcs
               .Where(x => x.WhsId == _whs.Id)
               .ToList());
        }

        public bool Add(Invoice invoice)
        {
            if (invoice == null || invoice.Items.Count == 0 || invoice.WhsId != _whs.Id)
                return false;

            _context.Invcs.Add(invoice);
            return true;
        }

        public bool Delete(Invoice invoice)
        {
            if (invoice == null || invoice.WhsId != _whs.Id)
                return false;

            Invoice? target = _context.Invcs
                .FirstOrDefault(i => i.Id == invoice.Id);

            if (target == null)
                return false;

            _context.Invcs.Remove(target);
            return true;
        }

        public bool Edit(Invoice invoice)
            => false;

        public ApplicationContext GetContext()
            => _context;
        

        public Warehouse GetWarehouse()
            => _whs;
    }
}
