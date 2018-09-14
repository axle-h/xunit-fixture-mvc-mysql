using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Xunit.Fixture.Mvc.MySql
{
    internal class MySqlBootstrap<TDbContext> : ITestServerBootstrap
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly MySqlBootstrapDataContext _data;

        public MySqlBootstrap(TDbContext context, MySqlBootstrapDataContext data)
        {
            _context = context;
            _data = data;
        }

        public async Task BootstrapAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.MigrateAsync();

            if (_data.Data.Any())
            {
                await _context.AddRangeAsync(_data.Data);
                await _context.SaveChangesAsync();
            }
        }
    }
}
