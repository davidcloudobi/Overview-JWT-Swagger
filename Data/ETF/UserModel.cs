using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Data.ETF
{
   public class UserModel:IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
