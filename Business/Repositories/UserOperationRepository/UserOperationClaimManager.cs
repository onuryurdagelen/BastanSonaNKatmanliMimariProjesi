using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects;
using Core.Aspects.Transaction;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserService _userService;

        public UserOperationClaimManager(IUnitOfWork unitOfWork, IOperationClaimService operationClaimService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _operationClaimService = operationClaimService;
            _userService = userService;
        }

        public IResult Add(UserOperationClaim userOperationClaim)
        {
            IResult result = BusinessRules.Run(
                IsOperationSetExist(userOperationClaim),
                IsUserExist(userOperationClaim.UserId),
                IsOperationClaimExistForAdd(userOperationClaim.OperationClaimId)
                );
            if (result !=null)
            {
                return result;
            }
            _unitOfWork.UserOperationClaims.Add(userOperationClaim);
            _unitOfWork.Complete();
            return new SuccessResult(UserOperationClaimMessages.AddedUserOperationClaim);
        }

        public IResult Delete(UserOperationClaim userOperationClaim)
        {
            throw new NotImplementedException();
        }

        public IDataResult<UserOperationClaim> GetById(int ocId)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<UserOperationClaim>> GetList()
        {
            throw new NotImplementedException();
        }
        [ValidationAspect(typeof(UserOperationClaimValidator))]
        [TransactionAspect()]
        public IResult Update(UserOperationClaim userOperationClaim)
        {
            var result = BusinessRules.Run(IsOperationSetExistForUpdate(userOperationClaim));
            if (result !=null)
            {
                return result;
            }
            _unitOfWork.UserOperationClaims.Update(userOperationClaim);
            return new SuccessResult(UserOperationClaimMessages.UpdatedUserOperationClaim);
        }
        public IResult IsOperationSetExist(UserOperationClaim userOperationClaim)
        {
            var result = _unitOfWork.UserOperationClaims.Get(uoc => uoc.UserId == userOperationClaim.UserId && uoc.OperationClaimId == userOperationClaim.OperationClaimId);

            if (result !=null)
            {
                return new ErrorResult(UserOperationClaimMessages.OperationClaimSetExist);
            }
            return new SuccessResult();
        }
        public IResult IsUserExist(int userId)
        {
            var result = _userService.GetById(userId).Data;

            if (result != null)
            {
                return new SuccessResult();
            }
                return new ErrorResult(UserOperationClaimMessages.UserNotExist);
        }
        public IResult IsOperationClaimExistForAdd(int operationClaimId)
        {
            var result = _operationClaimService.GetById(operationClaimId).Data;
            if (result == null)
            {
                return new ErrorResult(UserOperationClaimMessages.OperationClaimSetExist);
            }
                return new SuccessResult();
        }
        private IResult IsOperationSetExistForUpdate(UserOperationClaim userOperationClaim)
        {
            var currentUserOperationClaim = _unitOfWork.UserOperationClaims.Get(p => p.Id == userOperationClaim.Id);
            if (currentUserOperationClaim.UserId != userOperationClaim.UserId || 
                currentUserOperationClaim.OperationClaimId != userOperationClaim.OperationClaimId)
            {
                //OperationClaimId ya da UserId değiştiyse
                var result = _unitOfWork.UserOperationClaims.Get(p => p.UserId == userOperationClaim.UserId &&
                p.OperationClaimId == userOperationClaim.OperationClaimId);
                if (result != null)
                {
                    return new ErrorResult(UserOperationClaimMessages.OperationClaimSetExist);
                }
              
            }
            return new SuccessResult();
        }
    }
}
