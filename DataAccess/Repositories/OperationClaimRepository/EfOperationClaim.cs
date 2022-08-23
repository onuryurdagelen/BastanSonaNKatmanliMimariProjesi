using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EfOperationClaim : EfEntityRepositoryBase<OperationClaim>,IOperationClaimDal
    {
        public EfOperationClaim(DbContext context) : base(context)
        {
      
        }
    }
}
