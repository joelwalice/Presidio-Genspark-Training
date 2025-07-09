using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalAPI.Contexts;
using JobPortalAPI.Models;
using Microsoft.EntityFrameworkCore;
using JobPortalAPI.Interfaces;

namespace JobPortalAPI.Repositories
{
    public class UserRepository : IRepository<Guid, User>
    {
        private readonly JobContexts _context;
        public UserRepository(JobContexts context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.JobSeeker)
                    .Include(u => u.Recruiter)
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception)
            {
                throw new Exception("Error retrieving user by ID.");
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users
                    .Include(u => u.JobSeeker)
                    .Include(u => u.Recruiter)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error retrieving all users.");
            }
        }

        public async Task<User> AddAsync(User entity)
        {
            try
            {
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                throw new Exception("Error adding user.");
            }
        }

        public async Task<User> UpdateAsync(User entity)
        {
            try
            {
                _context.Users.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                throw new Exception("Error updating user.");
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return false;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error deleting user.");
            }
        }
    }
}