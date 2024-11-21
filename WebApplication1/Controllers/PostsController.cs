using App.Business.Dto;
using App.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // PUT api/<PostsController>/5
        [HttpPut("UpdatePost")]
        public async Task<IActionResult> Put([FromBody] PostDto postDto)
        {
            await postService.AddAsync(postDto);
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
