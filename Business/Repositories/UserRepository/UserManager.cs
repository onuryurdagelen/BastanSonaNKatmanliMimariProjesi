using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects;
using Core.Aspects.Caching;
using Core.Aspects.Transaction;
using Core.Aspects.Validation;
using Core.Extensions;
using Core.Utilities.Result;
using Core.Utilities.Security;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserManager:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        public UserManager(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        [TransactionAspect()]
        [RemoveCacheAspect("IUserService.GetList")]
        public IResult Add(RegisterDto registerDto)
        {

            string fileName = _fileService.SaveFileToServer(registerDto.Image, "./Assets/images/");

            byte[] fileByteArray = _fileService.SaveConvertByteArrayToDatabase(registerDto.Image);

            string ftpFile = _fileService.SaveFileToFtp(registerDto.Image);

            var user = CreateUser(registerDto, ftpFile);

            _unitOfWork.UserDals.Add(user);
            _unitOfWork.Complete();
            return new SuccessResult("Kullanıcı Ekleme işlemi başarılı");
        }
        [TransactionAspect()]
        private User CreateUser(RegisterDto registerDto,string fileName)
        {
            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            User user = new User();
            user.Id = 0;
            user.Email = registerDto.Email;
            user.Name = registerDto.Name;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ImageUrl = fileName;

            return user;
        }
        public User GetUserByEmail(string email)
        {
            var result = _unitOfWork.UserDals.Get(u => u.Email == email);

            return result;
           
          
        }

        [CacheAspect(60)]
        public IDataResult<List<User>> GetList()
        {
            var result =_unitOfWork.UserDals.GetAll();
           
            return new SuccessDataResult<List<User>>(result, UserMessages.SuccessUserListMessage);
        }

        [ValidationAspect(typeof(UserValidator))] //İşlemden önce çalışır.
        [TransactionAspect()]
        public IResult Update(User user)
        {
            _unitOfWork.UserDals.Update(user);
            _unitOfWork.Complete();
            return new SuccessResult(UserMessages.UpdatedUser);
        }
        [TransactionAspect()]
        public IResult Delete(User user)
        {
            _unitOfWork.UserDals.Delete(user);
            _unitOfWork.Complete();

            return new SuccessResult(UserMessages.DeletedUser);
        }

        public IDataResult<User> GetById(int userId)
        {
            var result = _unitOfWork.UserDals.Get(u => u.Id == userId);
           
            return new SuccessDataResult<User>(result);
        }
        [TransactionAspect()]
        public IResult ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            byte[] passwordHash, passwordSalt;
            var checkedUser = _unitOfWork.UserDals.Get(u => u.Id == userChangePasswordDto.UserId);

            var result = HashingHelper.VerifyPasswordHash(userChangePasswordDto.OldPassword, checkedUser.PasswordHash, checkedUser.PasswordSalt);

            if (!result)
               return new ErrorResult(UserMessages.WrongOldPassword);

            
            HashingHelper.CreatePasswordHash(userChangePasswordDto.NewPassword, out passwordHash, out passwordSalt);

            checkedUser.PasswordHash = passwordHash;
            checkedUser.PasswordSalt = passwordSalt;
            _unitOfWork.UserDals.Update(checkedUser);
            _unitOfWork.Complete();

            return new SuccessResult(UserMessages.PasswordChanged);
       
          
        }

        public List<OperationClaim> GetOperationClaimsById(int userId)
        {
            return _unitOfWork.UserDals.GetUserOperationClaims(userId);

            
        }
    }
}
