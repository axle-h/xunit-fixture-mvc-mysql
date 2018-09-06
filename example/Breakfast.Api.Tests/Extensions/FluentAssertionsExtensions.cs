using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;

namespace Breakfast.Api.Tests.Extensions
{
    public static class FluentAssertionsExtensions
    {
        public static AndConstraint<GenericCollectionAssertions<TModel>> HaveEquivalentProperty<TModel, TProperty>(this GenericCollectionAssertions<TModel> assertions,
                                                                                                                   IEnumerable<TModel> expected,
                                                                                                                   Func<TModel, TProperty> propertySelector)
        {
            assertions.Subject.Select(propertySelector).Should().BeEquivalentTo(expected.Select(propertySelector));
            return new AndConstraint<GenericCollectionAssertions<TModel>>(assertions);
        }
    }
}