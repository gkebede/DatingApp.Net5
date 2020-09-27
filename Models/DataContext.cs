using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_API.Models
{
    public class DataContext : DbContext 

    {
        public DataContext(DbContextOptions<DataContext> options)
                    : base(options)
        {
        }
      public DbSet<AppUsers> AppUsers  { get; set; }
    }
}
