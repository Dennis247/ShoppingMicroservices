using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Common.Settings
{
    public class RabbitMQSettings
    {
        public string Host { get; init; }
        public string Username { get;set; }
        public string Password { get; set; }
    }
}