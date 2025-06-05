using System;

namespace DocumentSharing.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> GetById(K id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(K id, T entity);
        Task<bool> Delete(K id);
    }
}