using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Repository.Inteface
{
    public interface IGenericRepository<T>
    {
        List<T> GetAll();
        bool Create(T entity);
        void Delete(T entity);
        bool Update(T entity);
    }
}
