using App.Business.Exceptions;
using App.Data.Contexts;
using AutoMapper;
using System;
using System.Collections;
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
            CheckEntityExists(id);
            await unitOfWork.Repository<TEntity>().DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();

        }

        public async Task UpdateAsync(TDto entity)
        {
            int entityId = Convert.ToInt32(entity.GetType().GetProperties().First(n => n.Name.Contains("Id"))
                .GetValue(entity));
            CheckEntityExists(entityId);
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
                    throw new BusinessExceptionHandler(ErrorStatusCode.FailedToGetData,$"No {typeof(TDto).Name}s were found");
                }

            }
            catch (EntityNotFoundException ex)
            {
                var message = $"Error retrieving all {typeof(TDto).Name}s";

                throw new BusinessExceptionHandler(ErrorStatusCode.FailedToGetData, $"No {typeof(TDto).Name}s were found");
            }
        }

        public async Task<TDto> GetAsync(int id)
        {
            var result =  unitOfWork.Repository<TEntity>().GetById(id);
            CheckEntityExists(id);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<TDto>(result);
        }

        private void CheckEntityExists(int id)
        {
            TEntity result =  unitOfWork.Repository<TEntity>().GetById(id);
            if (result is null)
                throw new BusinessExceptionHandler(ErrorStatusCode.PostNotExist, "Object not exsist");
        }

        public async Task<IEnumerable<TDto>> GetAllAsyncByPage(int page,int count)
        {
            IEnumerable<TDto> list = await GetAllAsync();
            return list
                .Skip((page - 1) * count)
                .Take(count)
                .ToList();
        }
    }
}
