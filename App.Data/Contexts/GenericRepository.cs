using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Contexts
{
    public interface IGenericRepository<T>
    {
        Task AddAsync(T entity); 
        void UpdateAsync(T entity);
        Task<List<T>> GetAllAsync();   
        Task DeleteAsync(int id);
        Task<T> GetById(int id);

    }
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext _context)
        {
            this._context = _context;
            _dbSet = _context.Set<T>();

        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
           


        }

        public async Task DeleteAsync(int id)
        {
            T result =   await _dbSet.FindAsync(id);
            if (result != null)
            {
                _dbSet.Remove(result);
            
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            IQueryable<T> query = _dbSet;
            query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
            
        }


        

        public void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
    }

       
}
