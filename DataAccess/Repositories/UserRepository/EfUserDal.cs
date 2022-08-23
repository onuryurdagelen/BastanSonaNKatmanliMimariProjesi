using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfUserDal : EfEntityRepositoryBase<User>,IUserDal
    {
        public EfUserDal(DbContext context) : base(context)
        {
        }

        public List<OperationClaim> GetUserOperationClaims(int userId)
        {
            using (var demoDbContext = new DemoDbContext())
            {
                var result = from userOperationClaim in demoDbContext.UserOperationClaims.Where(p => p.UserId == userId)
                             join operationClaim in demoDbContext.OperationClaims on userOperationClaim.OperationClaimId equals operationClaim.Id
                             select new OperationClaim
                             {
                                 Id = operationClaim.Id,
                                 Name = operationClaim.Name
                             };
                return result.OrderBy(p => p.Name).ToList();
            }
        }

        //public Task<bool> SignInWithEmailAsync(string email)
        //{
        //    var query = _context.Users.Any

        //    return query;
        //}
    }
}
