using DataAccess.Repositories.EmailParameterRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUnitOfWork: IDisposable
    {
        int Complete();
        IUserDal UserDals { get; }
        IOperationClaimDal OperationClaims { get; }
        IUserOperationClaimDal UserOperationClaims { get; }
        IEmailParameterDal EmailParameterDal { get; }   
    }
}
