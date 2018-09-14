using System.Threading.Tasks;
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
        public async Task When_getting_all_existing_breakfast_items()
        {
            await new MvcFunctionalTestFixture<Startup>(_output)
                 .HavingConfiguredApiClient()
                 .HavingDatabaseWithMultipleBreakfastItems(out var items)
                 .WhenGetting(EntityNames.BreakfastItem)
                 .ShouldReturnSuccessfulStatus()
                 .ShouldReturnBreakfastItems(items)
                 .RunAsync();
        }

        [Fact]
        public async Task When_getting_existing_breakfast_item()
        {
            await new MvcFunctionalTestFixture<Startup>(_output)
                 .HavingConfiguredApiClient()
                 .HavingDatabaseWithSingleBreakfastItem(out var item)
                 .WhenGettingById(EntityNames.BreakfastItem, item.Id)
                 .ShouldReturnSuccessfulStatus()
                 .ShouldReturnBreakfastItem(item)
                 .RunAsync();
        }

        [Fact]
        public async Task When_creating_breakfast_item()
        {
            await new MvcFunctionalTestFixture<Startup>(_output)
                 .HavingConfiguredApiClient()
                 .HavingEmptyDatabase()
                 .WhenCreating(EntityNames.BreakfastItem, out CreateOrUpdateBreakfastItemRequest request)
                 .ShouldReturnSuccessfulStatus()
                 .ShouldReturnBreakfastItem(request.GetCreatedBreakfastItem(1))
                 .BreakfastItemShouldExistInDatabase(request.GetCreatedBreakfastItem(1))
                 .RunAsync();
        }

        [Fact]
        public async Task When_updating_breakfast_item()
        {
            await new MvcFunctionalTestFixture<Startup>(_output)
                 .HavingConfiguredApiClient()
                 .HavingDatabaseWithSingleBreakfastItem(out var item)
                 .WhenUpdating(EntityNames.BreakfastItem, item.Id, out CreateOrUpdateBreakfastItemRequest request)
                 .ShouldReturnSuccessfulStatus()
                 .ShouldReturnBreakfastItem(request.GetCreatedBreakfastItem(item.Id))
                 .BreakfastItemShouldExistInDatabase(request.GetCreatedBreakfastItem(item.Id))
                 .RunAsync();
        }

        [Fact]
        public async Task When_patching_breakfast_item()
        {
            var fixture = new MvcFunctionalTestFixture<Startup>(_output);
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

            await fixture.HavingConfiguredApiClient()
                         .HavingDatabaseWithBreakfastItems(item)
                         .WhenPatching(EntityNames.BreakfastItem, item.Id, request)
                         .ShouldReturnSuccessfulStatus()
                         .ShouldReturnBreakfastItem(expected)
                         .BreakfastItemShouldExistInDatabase(expected)
                         .RunAsync();
        }

        [Fact]
        public async Task When_deleting_breakfast_item()
        {
            await new MvcFunctionalTestFixture<Startup>(_output)
                 .HavingConfiguredApiClient()
                 .HavingDatabaseWithSingleBreakfastItem(out var item)
                 .WhenDeleting(EntityNames.BreakfastItem, item.Id)
                 .ShouldReturnSuccessfulStatus()
                 .BreakfastItemShouldNotExistInDatabase(item.Id)
                 .RunAsync();
        }
    }
}
