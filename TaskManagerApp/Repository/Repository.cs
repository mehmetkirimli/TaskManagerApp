using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagerApp.Data;
using TaskManagerApp.Repository.Impl;

namespace TaskManagerApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter = null)
        {
            if(filter != null) 
            { 
                return await _dbSet.Where(filter).ToListAsync();
            }
            else 
            { 
                return await _dbSet.ToListAsync();
            }
            return null;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                Log.Warning($"Entity with ID {id} not found in the database."); // Burada log yazılıyor
                return null; // Eğer entity bulunamazsa null döndürüyoruz
            }
            return entity;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            Log.Information($"Entity of type {typeof(T).Name} added successfully.");
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            Log.Information($"Entity of type {typeof(T).Name} updated successfully.");
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                Log.Warning($"Entity with ID {id} not found in the database for deletion.");
                return;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            Log.Information($"Entity with ID {id} deleted successfully.");
        }
    }
}
