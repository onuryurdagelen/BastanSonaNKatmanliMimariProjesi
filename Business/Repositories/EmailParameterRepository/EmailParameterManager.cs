using Business.Constants.Messages;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Business.Repositories.EmailParameterRepository
{
    public class EmailParameterManager:IEmailParameterService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmailParameterManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IResult Add(EMailParameter eMailParameter)
        {
            _unitOfWork.EmailParameterDal.Add(eMailParameter);
            _unitOfWork.Complete();
            return new SuccessResult(EmailParameterMessages.AddedEmail);
        }

        public IResult Delete(EMailParameter eMailParameter)
        {
            _unitOfWork.EmailParameterDal.Delete(eMailParameter);
            _unitOfWork.Complete();
            return new SuccessResult(EmailParameterMessages.DeletedEmail);
        }

        public IDataResult<EMailParameter> GetById(int emailId)
        {
            return new SuccessDataResult<EMailParameter>(_unitOfWork.EmailParameterDal.Get(p => p.Id == emailId));
        }

        public IDataResult<List<EMailParameter>> GetList()
        {
            return new SuccessDataResult<List<EMailParameter>>(_unitOfWork.EmailParameterDal.GetAll());
        }

        public IResult SendEmail(EMailParameter eMailParameter, string body, string subject, string emails)
        {
            using (MailMessage mail = new MailMessage())
            {
                string[] setEmails = emails.Split(","); 
                mail.From = new MailAddress(eMailParameter.Email); //Mail'i gönderecek kişi
                foreach (var email in setEmails)
                {
                    mail.To.Add(email);
                }
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = eMailParameter.IsBodyHtml;
                //mail.Attachments.Add() //Dosya eklemek için
                using (SmtpClient smtp = new SmtpClient(eMailParameter.Smtp))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(eMailParameter.Email, eMailParameter.Password);
                    smtp.EnableSsl = eMailParameter.SSL;
                    smtp.Port = eMailParameter.Port;
                    smtp.Send(mail);
                }
            }
            return new SuccessResult(EmailParameterMessages.EmailSendSuccessfully);
        }

        public IResult Update(EMailParameter eMailParameter)
        {
            _unitOfWork.EmailParameterDal.Update(eMailParameter);
            _unitOfWork.Complete();
            return new SuccessResult(EmailParameterMessages.UpdatedEmail);
        }
    }
}
