using App.Business.Dto;
using App.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : GenericController<PostDto>
    {
        private readonly IPostService postService;
        public PostController(IPostService postService):base(postService) 
        {
            this.postService = postService;
        }
        //[HttpGet]
        //public async Task<IActionResult> GetPosts()
        //{
        //    List<PostDto> posts = await postService.GetPostByUserId(UserId);
        //    return Ok(posts);
        //}
        [HttpGet("ByPage")]
        public override async Task<IActionResult> Get(int page,int count)
        {
            List<PostDto> posts = await postService.GetPostByUserId(GetUserId(), page, count);
            return Ok(posts);
        }
        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] PostDto postDto)
        {
            postDto.UserId = GetUserId();
            await postService.AddAsync(postDto);
            return Ok(new { Message = "Post added" });
        }
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] CommentViewModel commentView)
        {
            int userId = Convert.ToInt32(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            CommentDto commentDto = new CommentDto();
            commentDto.UserId = GetUserId();
            commentDto.Description = commentView.Description;
            commentDto.PostId = commentView.PostId;
            await postService.AddComment(commentDto);
            return Ok(new { Message = "Comment added" });
        }
        [HttpPost("SetLike")]
        public async Task<IActionResult> SetLike([FromBody] LikeViewModel like)
        {
            LikeDto likeDto = new LikeDto();
            likeDto.UserId = GetUserId();
            likeDto.PostId = like.PostId;
            await postService.Like(likeDto);
            return Ok(new { Message = "Like was sent" });
        }

    }
}
