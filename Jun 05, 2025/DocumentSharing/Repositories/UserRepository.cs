using System;
using DocumentSharing.Models;
using DocumentSharing.Contexts;
using DocumentSharing.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentSharing.Repositories
{
    public class Repository
    {
        public readonly NotifyContext _context;
        public Repository(NotifyContext context)
        {
            _context = context;
        }
        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(int id, User document)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            existingUser.Name = document.Name; 
            existingUser.Email = document.Email; 
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}