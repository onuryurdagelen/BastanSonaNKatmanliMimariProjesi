using Core.Utilities.Result;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IResult Register(RegisterDto registerDto);

        IDataResult<Token> Login(LoginDto loginDto);
    }
}
