using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo.Models
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<DbContext> options)  :base(options)
        {

        }
        public DbSet<Users> users { get; set; }
    }
}
