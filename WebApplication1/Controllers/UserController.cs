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
    public class UserController : GenericController<UserDto>
    {
        private readonly IUserService userService;
        private readonly IJwtService jwtService;
        public UserController(IUserService userService,IJwtService jwtService):base(userService)
        {
            this.userService = userService;
            this.jwtService = jwtService;
        }

        [Authorize]
        [HttpGet("GetUser")]
        public async Task<UserDto> GetUser()
        {
            return await userService.GetUserDto(GetUserId());
        }
        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            userDto.Password = HashService.HashString(userDto.Password);
            await userService.AddUser(userDto);
            return Ok(new {Message ="User successfully added"});
        }
        [AllowAnonymous]
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
        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] UserDto userDto)
        {
            userDto.Password = HashService.HashString(userDto.Password);
            await userService.UpdateAsync(userDto);
            return Ok(userDto);
        }
    }
}
