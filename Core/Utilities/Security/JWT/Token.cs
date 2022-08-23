using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string RefreshToken { get; set; }



    }
}
