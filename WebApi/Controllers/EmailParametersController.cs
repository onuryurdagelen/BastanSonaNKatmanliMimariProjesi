using Business.Repositories.EmailParameterRepository;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailParametersController : ControllerBase
    {
        private readonly IEmailParameterService _emailParameterService;

        public EmailParametersController(IEmailParameterService emailParameterService)
        {
            _emailParameterService = emailParameterService;
        }
        [HttpPost("add")]
        public IActionResult AddOperationClaim(EMailParameter eMailParameter)
        {
            var result = _emailParameterService.Add(eMailParameter);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getEmailParameters")]
        //Token'ı varsa ve Rolü Admin ise  bu metot çalışır.
        //403 Error ise giriş yapılmış fakat yetki yok hatasıdır.
        public IActionResult GetOperationClaims()
        {
            var result = _emailParameterService.GetList();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("sendEmail")]
        public IActionResult SendEmail(EMailParameter eMailParameter, string body, string subject, string emails)
        {
            var result = _emailParameterService.SendEmail(eMailParameter,body, subject, emails);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
