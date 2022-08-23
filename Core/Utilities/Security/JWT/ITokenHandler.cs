using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHandler
    {
        /// <summary>
        /// Token oluşturma metotu kurduk.Bu metot Token objesi döner.Bu metot'a User objesi ve yetkilerini kontrol etmek için
        /// OperationClaim Listesi parametresi eklenir.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="operationClaims"></param>
        /// <returns>Token objesi döner</returns>
        Token CreateToken(User user, List<OperationClaim> operationClaims);
    }
}
