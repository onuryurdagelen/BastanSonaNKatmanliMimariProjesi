using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserDal:IEntityRepository<User>
    {
        //Task<bool> SignInWithEmailAsync(string email);

        List<OperationClaim> GetUserOperationClaims(int userId);
    }
}
