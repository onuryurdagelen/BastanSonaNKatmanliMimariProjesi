using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects;
using Core.Aspects.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result;
using Core.Utilities.Security;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUnitOfWork _unitOfWork;

        public AuthManager(IUserService userService, ITokenHandler tokenHandler = null, IUnitOfWork unitOfWork = null)
        {
            _userService = userService;
            _tokenHandler = tokenHandler;
            _unitOfWork = unitOfWork;
        }

        public IDataResult<Token> Login(LoginDto loginDto)
        {
            var existedUser = _userService.GetUserByEmail(loginDto.email);
            if (existedUser != null)
            {
                var result = HashingHelper.VerifyPasswordHash(loginDto.password, existedUser.PasswordHash, existedUser.PasswordSalt);
                var userOperationClaims = _userService.GetOperationClaimsById(existedUser.Id);
                if (result)
                {
                    var token = new Token();


                    token = _tokenHandler.CreateToken(existedUser, userOperationClaims);
                    
                    return new SuccessDataResult<Token>(token,"Giriş Başarılı.");
                }
            }
            return new ErrorDataResult<Token>("Kullanıcı maili ya da şifresi yanlıştır.");
        }


        [ValidationAspect(typeof(AuthValidator))] //İşlemden önce çalışır.
        //[LogAspect] //İşlem bittikten sonra çalışır.
        public IResult Register(RegisterDto registerDto)
        {
            //Cross Cutting Concerns ==> Uygulamayı dikine kesmek.
            //AOP
        

            var result = BusinessRules.Run(CheckIfEmailExists(registerDto.Email),
                CheckIfImageSizeIsLessThanOneMb(registerDto.Image.Length), 
                CheckUploadedFile(registerDto.Image.FileName));

            if (result != null)
            {
                return result;
            }
            _userService.Add(registerDto);
            return new SuccessResult("Kullanıcı kaydı başarıyla oluşturuldu.");
        }



        private IResult CheckIfEmailExists(string email)
        {
            var result = _userService.GetUserByEmail(email);
            if (result != null)
            {
                return new ErrorResult("Bu e-mail adresi sistemde bulunmaktadır.");
            }
            return new SuccessResult();
        }
        private IResult CheckIfImageSizeIsLessThanOneMb(long imageSize)
        {
            decimal imgMBSize = Convert.ToDecimal(imageSize * 0.000001);
            if (imgMBSize > 1)
            {
                return new ErrorResult("Yüklediğiniz resmin boyutu en fazla 1MB olmalıdır.");
            }
            return new SuccessResult();
        }
        private IResult CheckUploadedFile(string fileName)
        {
            var ext = fileName.Substring(fileName.LastIndexOf("."));
            var extension = ext.ToLower();

            List<string> AllowedFileExtensions = new List<string>() { ".png", ".jpeg", ".gif", ".jpg" };

            if (!AllowedFileExtensions.Contains(extension))
            {
                return new ErrorResult("Yüklediğiniz resim formatı .gif, .jpeg, .jpg türlerinden birisi olmalıdır!");
            }
            return new SuccessResult();
        }
    }
}
