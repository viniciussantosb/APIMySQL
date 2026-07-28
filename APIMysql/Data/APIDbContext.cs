using APIMysql.Models;
using Microsoft.EntityFrameworkCore;

namespace APIMysql.Data
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Estado> Estado { get; set; }
    }
}

