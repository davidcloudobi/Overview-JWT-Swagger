using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
  public  class UserRegDTO
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
