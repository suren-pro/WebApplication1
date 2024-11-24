using App.Business.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T> : ControllerBase, IGenericController<T> where T : class
    {
        private readonly IGenericBusinessService<T> _service;
        public GenericController(IGenericBusinessService<T> service)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T t)
        {
            await _service.AddAsync(t);
            return Ok(new {Message = "Successfully added"});
        }
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { Message = "Successfully removed" });
        }
        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
           return Ok(await _service.GetAllAsync());
        }
        [HttpGet("ByPage")]
        public virtual async Task<IActionResult> Get(int page, int count)
        {
            return Ok(await _service.GetAllAsyncByPage(page,count));
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(T t)
        {
            await _service.UpdateAsync(t);
            return Ok(new { Message = "Successfully updated" });

        }
        private protected int GetUserId()
        {
            int userId = Convert.ToInt32(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            return userId; ;
        }
    }
}
