using System;
using System.Collections.Generic;

namespace Xunit.Fixture.Mvc.MySql
{
    internal class MySqlBootstrapDataContext : IMySqlBootstrapDataContext
    {
        public IList<(object entity, Type type)> Data { get; } = new List<(object entity, Type type)>();

        public IMySqlBootstrapDataContext Add<TEntity>(TEntity entity)
        {
            Data.Add((entity, typeof(TEntity)));
            return this;
        }

        public IMySqlBootstrapDataContext AddRange<TEntity>(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Data.Add((entity, typeof(TEntity)));
            }

            return this;
        }
    }
}