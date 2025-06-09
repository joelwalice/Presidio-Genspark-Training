using System;

namespace JobPortalAPI.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> GetByIdAsync(K id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(K id);
    }
}