using System.Collections.Generic;

namespace Xunit.Fixture.Mvc.MySql
{
    internal class MySqlBootstrapDataContext : IMySqlBootstrapDataContext
    {
        public IList<object> Data { get; } = new List<object>();

        public IMySqlBootstrapDataContext Add<TEntity>(TEntity entity)
        {
            Data.Add(entity);
            return this;
        }

        public IMySqlBootstrapDataContext AddRange<TEntity>(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Data.Add(entity);
            }

            return this;
        }
    }
}