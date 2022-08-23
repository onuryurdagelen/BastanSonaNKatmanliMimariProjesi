using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.EmailParameterRepository
{
    public class EfEmailParameterDal : EfEntityRepositoryBase<EMailParameter>, IEmailParameterDal
    {
        public EfEmailParameterDal(DbContext context) : base(context)
        {
        }
    }
}
