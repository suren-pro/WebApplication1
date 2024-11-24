using App.Business.Dto;
using App.Business.Exceptions;
using App.Data.Contexts;
using App.Data.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Services
{
    public interface IUserService : IGenericBusinessService<UserDto>
    {
        Task<UserDto?> Login(string username, string password);
        Task<UserDto> GetUserDto(int id);
        Task AddUser(UserDto userDto);
    }
    public class UserService : GenericService<User, UserDto>, IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork unitOfWork,IMapper mapper):base(unitOfWork,mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddUser(UserDto userDto)
        {
            await CheckUsernameExists(userDto);
            await AddAsync(userDto);
        }
        public async Task<UserDto> GetUserDto(int id)
        {
            UserDto userDto = await GetAsync(id);
            userDto.Password = string.Empty;
            return userDto;
        }

        public async Task<UserDto?> Login(string username, string password)
        {
            UserDto? userDto=null;
            User user;
            List<User> users = await unitOfWork.Repository<User>().GetAllAsync();
            if (users != null && users.Count >= 1)
            {
                user = users.SingleOrDefault(predicate: u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    userDto = mapper.Map<UserDto>(user);

                    userDto.Password = string.Empty;
                    return userDto;
                }
                
            }
            return userDto;
        }
        private async Task CheckUsernameExists(UserDto userDto)
        {
            List<User> users = await unitOfWork.Repository<User>().GetAllAsync();
            User user = users.SingleOrDefault(u=>u.Username == userDto.Username);
            if (user != null)
                throw new BusinessExceptionHandler(ErrorStatusCode.UsernameAlreadyExists, "Username already taken");

        }
    }

    
}
