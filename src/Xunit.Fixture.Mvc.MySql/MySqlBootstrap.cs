using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

            // Add all in turn but group adjacent entities of the same type together.
            using (var data = _data.Data.GetEnumerator())
            {
                Type currentType = null;
                while (data.MoveNext())
                {
                    if (data.Current == null)
                    {
                        continue;
                    }

                    if (currentType != null && currentType != data.Current.GetType())
                    {
                        await _context.SaveChangesAsync();
                    }

                    currentType = data.Current.GetType();
                    await _context.AddAsync(data.Current);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
