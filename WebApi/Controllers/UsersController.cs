using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //[HttpPost("add")]
        //public IActionResult AddUser([FromBody] AuthDto authDto)
        //{
        //    var result = _userService.Add(authDto);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _userService.GetList();
            if (result.Success)
                return Ok(result);
            else
               return BadRequest("Hata!");
        }
        [HttpGet("getbyid")]
        public IActionResult GetById(int userId)
        {
            var result = _userService.GetById(userId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }
        
        [HttpPost("changePassword")]
        public IActionResult ChangeUserPassword(UserChangePasswordDto userChangePasswordDto)
        {
            var result = _userService.ChangePassword(userChangePasswordDto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
