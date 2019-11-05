using BR.EF;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Utils
{
    public class DbInitializer
    {
        
        public static void SeedAsync(BRDbContext context)
        {
            if (!context.Admins.Any())
            {
               
                context.SaveChanges();
            }
        }
    }
}
