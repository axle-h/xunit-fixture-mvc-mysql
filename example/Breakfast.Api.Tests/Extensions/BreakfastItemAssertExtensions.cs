using System.Collections.Generic;
using Breakfast.Api.Data;
using Breakfast.Api.Entities;
using Breakfast.Api.Models;
using FluentAssertions;
using Xunit.Fixture.Mvc;
using Xunit.Fixture.Mvc.MySql.Extensions;

namespace Breakfast.Api.Tests.Extensions
{
    public static class BreakfastItemAssertExtensions
    {
        public static BreakfastItem GetCreatedBreakfastItem(this CreateOrUpdateBreakfastItemRequest request, int id) => new BreakfastItem
                                                                                                                        {
                                                                                                                            Id = id,
                                                                                                                            Name = request.Name,
                                                                                                                            Rating = request.Rating
                                                                                                                        };

        public static IMvcFunctionalTestFixture ShouldReturnBreakfastItem(this IMvcFunctionalTestFixture fixture, BreakfastItem expected) =>
            fixture.JsonResultShould<BreakfastItem>(r => r.Id.Should().Be(expected.Id),
                                                    r => r.Name.Should().Be(expected.Name),
                                                    r => r.Rating.Should().Be(expected.Rating));

        public static IMvcFunctionalTestFixture ShouldReturnBreakfastItems(this IMvcFunctionalTestFixture fixture, ICollection<BreakfastItem> expected) =>
            fixture.JsonResultShould<ICollection<BreakfastItem>>(rs => rs.Should().HaveSameCount(expected)
                                                                         .And.HaveEquivalentProperty(expected, x => x.Id)
                                                                         .And.HaveEquivalentProperty(expected, x => x.Name)
                                                                         .And.HaveEquivalentProperty(expected, x => x.Rating));

        public static IMvcFunctionalTestFixture BreakfastItemShouldExistInDatabase(this IMvcFunctionalTestFixture fixture, BreakfastItem expected) =>
            fixture.ShouldExistInDatabase<BreakfastContext, BreakfastItem>(expected.Id,
                                                                           existing => existing.Id.Should().Be(expected.Id),
                                                                           existing => existing.Name.Should().Be(expected.Name),
                                                                           existing => existing.Rating.Should().Be(expected.Rating));

        public static IMvcFunctionalTestFixture BreakfastItemShouldNotExistInDatabase(this IMvcFunctionalTestFixture fixture, int id) =>
            fixture.ShouldNotExistInDatabase<BreakfastContext, BreakfastItem>(id);
    }
}