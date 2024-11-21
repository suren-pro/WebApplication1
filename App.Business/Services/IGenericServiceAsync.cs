using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Services
{
    public interface IGenericServiceAsync<TEntity,TDto> where TEntity : class where TDto:class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(TDto entity);
        Task AddAsync(TDto dto);
    }
}
