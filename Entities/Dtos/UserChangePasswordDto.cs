using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class UserChangePasswordDto
    {
        public int  UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
