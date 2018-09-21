using System;
using System.Collections.Generic;
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

            // Add all in turn but group adjacent entities of the same type together.
            using (var data = _data.Data.GetEnumerator())
            {
                Type currentType = null;
                while (data.MoveNext())
                {
                    if (currentType != null && currentType != data.Current.type)
                    {
                        await _context.SaveChangesAsync();
                    }

                    currentType = data.Current.type;
                    await _context.AddAsync(data.Current.entity);
                }

                await _context.SaveChangesAsync();
            }

        }
    }
}
