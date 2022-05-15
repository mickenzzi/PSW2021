using System.Collections.Generic;

namespace PSW.Repository.Interface
{
    public interface IGenericRepository<T>
    {
        List<T> GetAll();
        bool Create(T entity);
        void Delete(T entity);
        bool Update(T entity);
    }
}