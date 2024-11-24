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
    public interface IPostService:IGenericBusinessService<PostDto>
    {
        Task<List<PostDto>> GetPostByUserId(int userId);
        Task<List<PostDto>> GetPostByUserId(int userId,int page,int count);
        Task AddComment(CommentDto commentDto);
        Task Like(LikeDto likeDto);
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

        public async Task AddComment(CommentDto commentDto)
        {
            CheckPostExists(commentDto.PostId);
            await CheckUserExists(commentDto.UserId);
            Comment comment = mapper.Map<Comment>(commentDto);
            await unitOfWork.Repository<Comment>().AddAsync(comment);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<List<PostDto>> GetPostByUserId(int userId)
        {
            List<PostDto> postsDtos = new List<PostDto>();
            User user = await CheckUserExists(userId);
            List<Post> posts = await unitOfWork.Repository<Post>().GetAllAsync(
                p=>p.Comments,
                p=> p.Likes);
            posts = posts.Where(p => p.UserId == user.UserId).ToList();
            postsDtos = mapper.Map<List<PostDto>>(posts);
            return postsDtos;
        }

        public async Task<List<PostDto>> GetPostByUserId(int userId, int page, int count)
        {
            await CheckUserExists(userId);
            List<PostDto> posts = await GetPostByUserId(userId);
            return posts
                .Skip((page - 1)*count)
                .Take(count)
                .ToList();

        }

        public async Task Like(LikeDto likeDto)
        {
            CheckPostExists(likeDto.PostId);
            await CheckUserExists(likeDto.UserId);
            Like like = mapper.Map<Like>(likeDto);
            await unitOfWork.Repository<Like>().AddAsync(like);
            await unitOfWork.SaveChangesAsync();
        }


        private async Task<User> CheckUserExists(int userId)
        {
            User user =  unitOfWork.Repository<User>().GetById(userId);
            if (user is null)
            {
                throw new BusinessExceptionHandler(ErrorStatusCode.UserNotExist, "User not exsist");
            }
            return user;
        }
        private  Post CheckPostExists(int postId)
        {
            Post post = unitOfWork.Repository<Post>().GetById(postId);
            if (post is null)
            {
                throw new BusinessExceptionHandler(ErrorStatusCode.PostNotExist, "Post not exsist");
            }
            return post;
        }
    }
}
