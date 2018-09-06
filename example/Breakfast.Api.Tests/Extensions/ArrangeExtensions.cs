using System.Collections.Generic;
using System.Linq;
using Breakfast.Api.Data;
using Breakfast.Api.Entities;
using Xunit.Fixture.Mvc;
using Xunit.Fixture.Mvc.Extensions;
using Xunit.Fixture.Mvc.MySql.Extensions;

namespace Breakfast.Api.Tests.Extensions
{
    public static class ArrangeExtensions
    {
        public static IMvcFunctionalTestFixture HavingConfiguredApiClient(this IMvcFunctionalTestFixture fixture) => fixture.HavingPathBase("api");

        public static IMvcFunctionalTestFixture HavingEmptyDatabase(this IMvcFunctionalTestFixture fixture) => fixture.HavingMySqlDatabase<BreakfastContext>();

        public static IMvcFunctionalTestFixture HavingDatabaseWithBreakfastItems(this IMvcFunctionalTestFixture fixture, params BreakfastItem[] items) =>
            fixture.HavingMySqlDatabase<BreakfastContext>(c => c.AddRange(items));

        public static IMvcFunctionalTestFixture HavingDatabaseWithSingleBreakfastItem(this IMvcFunctionalTestFixture fixture, out BreakfastItem item)
        {
            item = fixture.Create<BreakfastItem>();
            return fixture.HavingDatabaseWithBreakfastItems(item);
        }

        public static IMvcFunctionalTestFixture HavingDatabaseWithMultipleBreakfastItems(this IMvcFunctionalTestFixture fixture, out ICollection<BreakfastItem> items)
        {
            items = fixture.CreateMany<BreakfastItem>();
            return fixture.HavingDatabaseWithBreakfastItems(items.ToArray());
        }
    }
}