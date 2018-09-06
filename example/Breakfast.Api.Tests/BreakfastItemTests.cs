using AutoFixture;
using Xunit.Fixture.Mvc;
using Xunit.Abstractions;
using Breakfast.Api.Entities;
using Breakfast.Api.Models;
using Breakfast.Api.Tests.Extensions;
using Xunit;
using Xunit.Fixture.Mvc.Extensions;

namespace Breakfast.Api.Tests
{
    public class BreakfastItemTests
    {
        private readonly ITestOutputHelper _output;

        public BreakfastItemTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void When_getting_all_existing_breakfast_items()
        {
            using (var fixture = new MvcFunctionalTestFixture<Startup>(_output))
            {
                fixture.HavingConfiguredApiClient()
                       .HavingDatabaseWithMultipleBreakfastItems(out var items)
                       .WhenGettingAll(EntityNames.BreakfastItem)
                       .ShouldReturnSuccessfulStatus()
                       .ShouldReturnBreakfastItems(items);
            }
        }

        [Fact]
        public void When_getting_existing_breakfast_item()
        {
            using (var fixture = new MvcFunctionalTestFixture<Startup>(_output))
            {
                fixture.HavingConfiguredApiClient()
                       .HavingDatabaseWithSingleBreakfastItem(out var item)
                       .WhenGettingById(EntityNames.BreakfastItem, item.Id)
                       .ShouldReturnSuccessfulStatus()
                       .ShouldReturnBreakfastItem(item);
            }
        }

        [Fact]
        public void When_creating_breakfast_item()
        {
            using (var fixture = new MvcFunctionalTestFixture<Startup>(_output))
            {
                fixture.HavingConfiguredApiClient()
                       .HavingEmptyDatabase()
                       .WhenCreating(EntityNames.BreakfastItem, out CreateOrUpdateBreakfastItemRequest request)
                       .ShouldReturnSuccessfulStatus()
                       .ShouldReturnBreakfastItem(request.GetCreatedBreakfastItem(1))
                       .BreakfastItemShouldExistInDatabase(request.GetCreatedBreakfastItem(1));
            }
        }

        [Fact]
        public void When_updating_breakfast_item()
        {
            using (var fixture = new MvcFunctionalTestFixture<Startup>(_output))
            {
                fixture.HavingConfiguredApiClient()
                       .HavingDatabaseWithSingleBreakfastItem(out var item)
                       .WhenUpdating(EntityNames.BreakfastItem, item.Id, out CreateOrUpdateBreakfastItemRequest request)
                       .ShouldReturnSuccessfulStatus()
                       .ShouldReturnBreakfastItem(request.GetCreatedBreakfastItem(item.Id))
                       .BreakfastItemShouldExistInDatabase(request.GetCreatedBreakfastItem(item.Id));
            }
        }

        [Fact]
        public void When_patching_breakfast_item()
        {
            using (var fixture = new MvcFunctionalTestFixture<Startup>(_output))
            {
                var item = fixture.Create<BreakfastItem>();

                var request = fixture.AutoFixture.Build<CreateOrUpdateBreakfastItemRequest>()
                                     .Without(x => x.Rating) // do not patch rating.
                                     .Create();

                var expected = new BreakfastItem
                               {
                                   Id = item.Id,
                                   Rating = item.Rating,
                                   Name = request.Name // only name should have changed.
                               };

                fixture.HavingConfiguredApiClient()
                       .HavingDatabaseWithBreakfastItems(item)
                       .WhenPatching(EntityNames.BreakfastItem, item.Id, request)
                       .ShouldReturnSuccessfulStatus()
                       .ShouldReturnBreakfastItem(expected)
                       .BreakfastItemShouldExistInDatabase(expected);
            }
        }

        [Fact]
        public void When_deleting_breakfast_item()
        {
            using (var fixture = new MvcFunctionalTestFixture<Startup>(_output))
            {
                fixture.HavingConfiguredApiClient()
                       .HavingDatabaseWithSingleBreakfastItem(out var item)
                       .WhenDeleting(EntityNames.BreakfastItem, item.Id)
                       .ShouldReturnSuccessfulStatus()
                       .BreakfastItemShouldNotExistInDatabase(item.Id);
            }
        }
    }
}
