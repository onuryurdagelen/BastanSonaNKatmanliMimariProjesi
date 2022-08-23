using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants.Enums
{
    public enum StatusCodes
    {
        Created = 201,
        Ok = 200,
        Accepted = 202,
        NoContent = 204,
        BadRequest = 400,
        UnAuthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,

        InternalServerError = 500,
        BadGateway= 502
    }
}
