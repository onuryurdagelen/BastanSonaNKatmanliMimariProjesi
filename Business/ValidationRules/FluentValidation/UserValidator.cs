using Entities.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("Kullanıcı adı boş olamaz.");
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email adresi boş olamaz.");
            RuleFor(u => u.Email).EmailAddress().WithMessage("Lütfen geçerli bir email adresi giriniz.");
            RuleFor(u => u.ImageUrl).NotEmpty().WithMessage("Şifre boş olamaz.");
           
        }
    }
}
