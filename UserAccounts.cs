using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUserProvisioning
{
    class UserAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public static UserAccount Create(string email, string password)
        {
            return new UserAccount
            {
                Email = email,
                Password = password
            };
            
        }
        
    }
}
