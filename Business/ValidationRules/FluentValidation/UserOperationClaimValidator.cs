using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class UserOperationClaimValidator:AbstractValidator<UserOperationClaim>
    {
        public UserOperationClaimValidator()
        {
            RuleFor(uoc => uoc.UserId).Must(IsIdValid).WithMessage("Yetki ataması için kullanıcı seçimi yapmalısınız.");
            RuleFor(uoc => uoc.OperationClaimId).NotEmpty().GreaterThan(0).WithMessage("Yetki ataması için operasyon yetki seçimi yapmalısınz");
        }
        private bool IsIdValid(int id)
        {
            if (id > 0 && id != null)
            {
                return true;
            }
            
            return false;
        }
    }
  
}
