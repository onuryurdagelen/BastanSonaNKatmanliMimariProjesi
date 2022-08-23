using Business.Abstract;
using Business.Constants;
using Business.Constants.Enums;
using Business.ValidationRules.FluentValidation;
using Core.Aspects;
using Core.Aspects.Performance;
using Core.Aspects.Secured;
using Core.Aspects.Transaction;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OperationClaimManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [TransactionAspect()]
        [ValidationAspect(typeof(OperationClaimValidator))]
        public IResult Add(OperationClaim operationClaim)
        {
            IResult result = BusinessRules.Run(IsNameAvailableForAdd(operationClaim.Name));
            if (result != null)
            {
                return result;
            }
            _unitOfWork.OperationClaims.Add(operationClaim);
            _unitOfWork.Complete();

            return new SuccessResult(OperationClaimMessages.AddedOperationClaim);
        }
        [TransactionAspect()]
        public IResult Delete(OperationClaim operationClaim)
        {
            _unitOfWork.OperationClaims.Delete(operationClaim);
            _unitOfWork.Complete();

            return new SuccessResult(OperationClaimMessages.DeletedOperationClaim);
        }
        [TransactionAspect()]
        public IDataResult<OperationClaim> GetById(int ocId)
        {
           var result = _unitOfWork.OperationClaims.Get(oc => oc.Id == ocId);
           
            return new SuccessDataResult<OperationClaim>(result);
        }
        [SecuredAspect()]
        [PerformanceAspect()]
        public IDataResult<List<OperationClaim>> GetList()
        {
            var result = _unitOfWork.OperationClaims.GetAll();
           
            return new SuccessDataResult<List<OperationClaim>>(result);
        }
        [TransactionAspect()]
        public IResult Update(OperationClaim operationClaim)
        {
            var result = BusinessRules.Run(IsNameAvailableForUpdate(operationClaim));
            if (result !=null)
            {
                return result;
            }
            _unitOfWork.OperationClaims.Update(operationClaim);
            _unitOfWork.Complete();

            return new SuccessResult(OperationClaimMessages.UpdatedOperationClaim) ;
        }
        public IResult IsNameAvailableForAdd(string name)
        {
            var result = _unitOfWork.OperationClaims.Get(oc => oc.Name.ToLower() == name.ToLower());
            if (result != null)
            {
                return new ErrorResult(OperationClaimMessages.NameIsNotAvailable);
            }
            return new SuccessResult();
        }
        public IResult IsNameAvailableForUpdate(OperationClaim operationClaim)
        {
            var currentOperationClaim = _unitOfWork.OperationClaims.Get(p => p.Id == operationClaim.Id);
            //Eski gönderdiğimiz ad ile yeni gönderdiğimiz ad farklı ise
            if (currentOperationClaim.Name.ToLower() != operationClaim.Name.ToLower())
            {
                //Girdiğimiz ad ile eşleşen bir claim adı var mı kontrol edilir.Var ise Hata mesajı yollanır;yok ise başarılı mesaj gönderilir.
                var result = _unitOfWork.OperationClaims.Get(oc => oc.Name.ToLower() == operationClaim.Name.ToLower());
                if (result != null)
                {
                    return new ErrorResult(OperationClaimMessages.NameIsNotAvailable);
                }
            }
            
            return new SuccessResult();
        }
    }
}
