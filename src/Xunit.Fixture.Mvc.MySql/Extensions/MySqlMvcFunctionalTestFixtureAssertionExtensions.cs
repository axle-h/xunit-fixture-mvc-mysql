using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Fixture.Mvc.Infrastructure;

namespace Xunit.Fixture.Mvc.MySql.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IMvcFunctionalTestFixture"/> for building assertions against a MySQL database.
    /// </summary>
    public static class MySqlMvcFunctionalTestFixtureAssertionExtensions
    {
        /// <summary>
        /// Adds an assertion to check that an entity of the specified type exists in the database, also running the specified assertions against it.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="assertions">The assertions.</param>
        /// <returns></returns>
        public static IMvcFunctionalTestFixture ShouldExistInDatabase<TDbContext, TEntity>(this IMvcFunctionalTestFixture fixture,
                                                                                           object id,
                                                                                           params Action<TEntity>[] assertions)
            where TDbContext : DbContext
            where TEntity : class =>
            fixture.PostRequestResolvedServiceShould<TDbContext>(async ctx =>
                                                                 {
                                                                     var existing = await ctx.Set<TEntity>().FindAsync(id);
                                                                     existing.Should().NotBeNull();

                                                                     using (var aggregator = new ExceptionAggregator())
                                                                     {
                                                                         foreach (var assertion in assertions)
                                                                         {
                                                                             aggregator.Try(() => assertion(existing));
                                                                         }
                                                                     }
                                                                 });

        /// <summary>
        /// Adds an assertion to check that an entity of the specified type does not exist in the database.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static IMvcFunctionalTestFixture ShouldNotExistInDatabase<TDbContext, TEntity>(this IMvcFunctionalTestFixture fixture, object id)
            where TDbContext : DbContext
            where TEntity : class =>
            fixture.PostRequestResolvedServiceShould<TDbContext>(async ctx =>
                                                                 {
                                                                     var existing = await ctx.Set<TEntity>().FindAsync(id);
                                                                     existing.Should().BeNull();
                                                                 });
    }
}