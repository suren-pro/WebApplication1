using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Services
{
    public  interface IGenericBusinessService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsyncByPage(int page,int count);
        Task<T> GetAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task AddAsync(T dto);
    }
}
