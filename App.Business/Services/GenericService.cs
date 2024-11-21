using App.Business.Exceptions;
using App.Data.Contexts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Services
{
    public class GenericService<TEntity, TDto> : IGenericServiceAsync<TEntity, TDto> where TDto : class
        where TEntity : class
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        
        public GenericService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task AddAsync(TDto dto)
        {
            await unitOfWork.Repository<TEntity>().AddAsync(mapper.Map<TEntity>(dto));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.Repository<TEntity>().DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();

        }

        public async Task UpdateAsync(TDto entity)
        {
            unitOfWork.Repository<TEntity>().UpdateAsync(mapper.Map<TEntity>(entity));
            await unitOfWork.SaveChangesAsync();

        }
        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            try
            {
                var result = await unitOfWork.Repository<TEntity>().GetAllAsync();

                if (result.Any())
                {
                    return mapper.Map<IEnumerable<TDto>>(result);
                }
                else
                {
                    throw new EntityNotFoundException($"No {typeof(TDto).Name}s were found");
                }

            }
            catch (EntityNotFoundException ex)
            {
                var message = $"Error retrieving all {typeof(TDto).Name}s";

                throw new EntityNotFoundException(message, ex);
            }
        }

        public async Task<TDto> GetAsync(int id)
        {
            try
            {
                var result = await unitOfWork.Repository<TEntity>().GetById(id);

                if (result is null)
                {
                    throw new EntityNotFoundException($"Entity with ID {id} not found.");
                }

                return mapper.Map<TDto>(result);
            }

            catch (EntityNotFoundException ex)
            {
                var message = $"Error retrieving {typeof(TDto).Name} with Id: {id}";

                throw new EntityNotFoundException(message, ex);
            }
        }
    }
}
