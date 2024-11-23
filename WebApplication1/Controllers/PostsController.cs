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
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService) 
        {
            this.postService = postService;
        }
        // GET: api/<PostsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PostsController>/5
        [HttpGet("GetPosts")]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                int userId = Convert.ToInt32(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
                List<PostDto> posts = await postService.GetPostByUserId(userId);
                return Ok(posts);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        // GET api/<PostsController>/5
        [HttpGet("GetPostsByPage")]
        public async Task<IActionResult> GetPosts(int page,int count)
        {
            try
            {
                int userId = Convert.ToInt32(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
                List<PostDto> posts = await postService.GetPostByUserId(userId,page,count);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<PostsController>
        [HttpPost("AddPost")]
        public async Task<IActionResult> Post([FromBody] PostDto postDto)
        {
            int userId = Convert.ToInt32(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            postDto.UserId = userId;
            await postService.AddAsync(postDto);
            return Ok(new { Message = "Post added" });
        }
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] CommentViewModel commentView)
        {
            int userId = Convert.ToInt32(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            CommentDto commentDto = new CommentDto();
            commentDto.UserId = userId;
            commentDto.Description = commentView.Description;
            commentDto.PostId = commentView.PostId;
            await postService.AddComment(commentDto);
            return Ok(new { Message = "Comment added" });
        }
        [HttpPost("SetLike")]
        public async Task<IActionResult> SetLike([FromBody] LikeViewModel like)
        {
            int userId = Convert.ToInt32(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            LikeDto likeDto = new LikeDto();
            likeDto.UserId = userId;
            likeDto.PostId = like.PostId;
            await postService.Like(likeDto);
            return Ok(new { Message = "Like was sent" });
        }

        // PUT api/<PostsController>/5
        [HttpPut("UpdatePost")]
        public async Task<IActionResult> Put([FromBody] PostDto postDto)
        {
            await postService.UpdateAsync(postDto);
            return Ok(new { Message = "Post updated" });
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("RemovePost/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await postService.DeleteAsync(id);
            return Ok(new { Message = "Post removed" });
        }
    }
}
