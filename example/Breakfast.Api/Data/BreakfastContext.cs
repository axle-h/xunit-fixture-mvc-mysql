using Breakfast.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Breakfast.Api.Data
{
    public class BreakfastContext : DbContext
    {
        public BreakfastContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BreakfastItem> BreakfastItems { get; set; }
    }
}
