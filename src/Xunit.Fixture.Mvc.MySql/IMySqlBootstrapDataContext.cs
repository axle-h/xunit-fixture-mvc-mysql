using System.Collections.Generic;

namespace Xunit.Fixture.Mvc.MySql
{
    /// <summary>
    /// A context for adding data to the bootstrapped MySQL database.
    /// </summary>
    public interface IMySqlBootstrapDataContext
    {
        /// <summary>
        /// Adds the specified entity to the bootstrapped database.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        IMySqlBootstrapDataContext Add<TEntity>(TEntity entity);

        /// <summary>
        /// Adds the specified entities to the bootstrapped database.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        IMySqlBootstrapDataContext AddRange<TEntity>(IEnumerable<TEntity> entities);
    }
}