using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserService
    {
        IResult Add(RegisterDto registerDto);
        IDataResult<List<User>> GetList();
        User GetUserByEmail(string email);

        IDataResult<User> GetById(int userId);

        IResult Update(User user);
        IResult Delete(User user);

        IResult ChangePassword(UserChangePasswordDto userChangePasswordDto);

        List<OperationClaim> GetOperationClaimsById(int userId);
    }
}
