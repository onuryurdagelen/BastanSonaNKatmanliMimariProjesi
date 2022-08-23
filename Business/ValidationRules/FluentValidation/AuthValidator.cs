using Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class AuthValidator:AbstractValidator<RegisterDto>
    {
        public AuthValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Kullanıcı adı boş olamaz.");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Email adresi boş olamaz.");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Lütfen geçerli bir email adresi giriniz.");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre boş olamaz.");
            RuleFor(p => p.Password).MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
            RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Şifre en az bir adet büyük harf içermelidir.");
            RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Şifre en az bir adet küçük harf içermelidir.");
            RuleFor(p => p.Password).Matches("[0-9]").WithMessage("Şifre en az bir adet sayı harf içermelidir.");
            RuleFor(p => p.Password).Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir adet özel karakter içermelidir.");
        }
    }
}
