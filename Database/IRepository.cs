using System;
using System.Collections.Generic;

namespace uRP.Database
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
        IEnumerable<T> GetAll();
        T GetByID(long id);
    }
}
