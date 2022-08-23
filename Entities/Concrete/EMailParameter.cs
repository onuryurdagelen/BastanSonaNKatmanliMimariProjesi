using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class EMailParameter:IEntity
    {
        public int Id { get; set; }
        public string Smtp { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int Port { get; set; }
        public bool SSL { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
