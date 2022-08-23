using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class EmailParameterValidator:AbstractValidator<EMailParameter>
    {
        public EmailParameterValidator()
        {
            RuleFor(x => x.Smtp).NotEmpty().WithMessage("SMTP adresi boş olamaz");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Mail Adresi boş olamaz.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş olamaz.");
            RuleFor(x => x.Port).NotEmpty().WithMessage("Port boş olamaz.");
        }
    }
}
