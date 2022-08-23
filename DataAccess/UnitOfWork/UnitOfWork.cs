using DataAccess.Abstract;
using DataAccess.Concrete.Contexts;
using DataAccess.Repositories.EmailParameterRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly DemoDbContext _context;
        private readonly EfUserDal _efUserDal;
        private readonly EfOperationClaim _efOperationClaim;
        private readonly EfUserOperationClaim _efUserOperationClaim;
        private readonly EfEmailParameterDal _efEmailParameterDal;
        public UnitOfWork(DemoDbContext context)
        {
            _context = context;
        }

        public IUserDal UserDals => _efUserDal ?? new EfUserDal(_context);

        public IOperationClaimDal OperationClaims => _efOperationClaim ?? new EfOperationClaim(_context);

        public IUserOperationClaimDal UserOperationClaims => _efUserOperationClaim ?? new EfUserOperationClaim(_context);

        public IEmailParameterDal EmailParameterDal => _efEmailParameterDal ?? new EfEmailParameterDal(_context);

        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
