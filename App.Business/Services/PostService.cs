﻿using App.Business.Dto;
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
    public interface IPostService:IGenericServiceAsync<Post,PostDto>
    {
        Task<List<PostDto>> GetPostByUserId(int userId);
        Task<List<PostDto>> GetPostByUserId(int userId,int page,int count);
    }
    public class PostService:GenericService<Post,PostDto>, IPostService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PostService(IUnitOfWork unitOfWork,IMapper mapper):base(unitOfWork,mapper) 
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<List<PostDto>> GetPostByUserId(int userId)
        {
            List<PostDto> postsDtos = new List<PostDto>();
            User user = await unitOfWork.Repository<User>().GetById(userId);
            if (user is null)
                throw new EntityNotFoundException("User not found");
            List<Post> posts = await unitOfWork.Repository<Post>().GetAllAsync();
            posts = posts.Where(p => p.UserId == user.UserId).ToList();
            postsDtos = mapper.Map<List<PostDto>>(posts);
            return postsDtos;
        }

        public async Task<List<PostDto>> GetPostByUserId(int userId, int page, int count)
        {
            List<PostDto> posts = await GetPostByUserId(userId);
            return posts
                .Skip((page - 1)*count)
                .Take(count)
                .ToList();

        }
    }
}
