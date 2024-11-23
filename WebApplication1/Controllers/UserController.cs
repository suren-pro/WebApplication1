using App.Business.Dto;
using App.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Service;
using WebApplication1.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IJwtService jwtService;

        public UserController(IUserService userService,IJwtService jwtService) 
        {
            this.userService = userService;
            this.jwtService = jwtService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<UserDto>> Get()
        {
            return await userService.GetAllAsync();
        }
        [Authorize]
        // GET api/<UserController>/5
        [HttpGet("GetUser")]
        public async Task<UserDto> GetUser()
        {
            int userId = Convert.ToInt32(User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            return await userService.GetUserDto(userId);
        }

        // POST api/<UserController>
        [HttpPost("AddUser")]
        public async Task<IActionResult> Post([FromBody] UserDto userDto)
        {
            userDto.Password = HashService.HashString(userDto.Password);
            await userService.AddUser(userDto);
            return Ok(new {Message ="User successfully added"});
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel userViewModel)
        {
            userViewModel.Password = HashService.HashString(userViewModel.Password);
            UserDto userDto = await userService.Login(userViewModel.Username,userViewModel.Password);
            if (userDto != null)
            {
                string token = jwtService.GenerateJwtToken(userDto);
                Response.Headers.Add("token", token);
                return Ok(userDto);
            }
            else
                return BadRequest(new {Message ="Login failed"});
        }

        // PUT api/<UserController>/5
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Put([FromBody] UserDto userDto)
        {
            userDto.Password = HashService.HashString(userDto.Password);
            await userService.UpdateAsync(userDto);
            return Ok(userDto);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("RemoveUser/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await userService.DeleteAsync(id);
            return Ok("User  was removed");
        }
    }
}
