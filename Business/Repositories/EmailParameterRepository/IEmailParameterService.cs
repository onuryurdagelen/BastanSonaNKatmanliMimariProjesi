using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Repositories.EmailParameterRepository
{
    public interface IEmailParameterService
    {
        IResult Add(EMailParameter eMailParameter);

        IResult Update(EMailParameter eMailParameter);

        IResult Delete(EMailParameter eMailParameter);

        IDataResult<List<EMailParameter>> GetList();

        IDataResult<EMailParameter> GetById(int emailId);

        IResult SendEmail(EMailParameter eMailParameter,string body,string subject, string emails);
    }
}
