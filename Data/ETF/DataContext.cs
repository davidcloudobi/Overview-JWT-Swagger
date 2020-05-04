using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.ETF
{
   public class DataContext:IdentityDbContext<UserModel>
    {


        public DbSet<PhoneStore> Phones { get; set; }

        public DataContext(DbContextOptions options): base(options)
        {

        }
        
    }
}
