using BankAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAPI.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly BankContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(BankContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetAsync(string accountNumber)
        {
            // If T has a property named AccountNumber, use it for lookup
            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<string>(e, "AccountNumber") == accountNumber);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(string id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                return false;
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}