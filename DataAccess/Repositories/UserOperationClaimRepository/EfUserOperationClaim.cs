using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class EfUserOperationClaim : EfEntityRepositoryBase<UserOperationClaim>,IUserOperationClaimDal
    {
        private DemoDbContext _context;
        public EfUserOperationClaim(DemoDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
