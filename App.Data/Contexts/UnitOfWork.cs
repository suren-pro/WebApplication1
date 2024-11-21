using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Contexts
{
    public interface IUnitOfWork 
    {
        Task SaveChangesAsync();
        IGenericRepository<T> Repository<T>() where T:class;
    }
    public class UnitOfWork: IUnitOfWork 
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IGenericRepository<T> Repository<T>() where T : class 
        {
            return new GenericRepository<T>(context);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
