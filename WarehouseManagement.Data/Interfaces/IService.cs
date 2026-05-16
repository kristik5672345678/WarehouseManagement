using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WarehouseManagement.Data.AbstractClasses;
using WarehouseManagement.Data.Context;

namespace WarehouseManagement.Data.Interfaces
{
    public interface IService<T>
        where T : ObjectModel
    {
        bool Add(T obj);
        bool Delete(T obj);
        bool Edit(T obj);
        ObservableCollection<T> GetAll();
        ApplicationContext GetContext();
    }
}
