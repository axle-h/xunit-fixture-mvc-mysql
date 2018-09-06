[![Build status](https://ci.appveyor.com/api/projects/status/mbl1tqk3gciah35u/branch/master?svg=true)](https://ci.appveyor.com/project/axle-h/xunit-fixture-mvc-mysql/branch/master)
[![NuGet](https://img.shields.io/nuget/v/xunit.fixture.mvc.mysql.svg)](https://www.nuget.org/packages/xunit.fixture.mvc.mysql)

# xunit-fixture-mvc-mysql

MVC functional tests with a fixture pattern for a MySql database on EF Core.

For example:

```C#
[Fact]
public void When_creating_breakfast_item()
{
    using (var fixture = new MvcFunctionalTestFixture<Startup>(_output))
    {
        var request = new CreateOrUpdateBreakfastItemRequest { Name = "bacon", Rating = 10 };
        fixture.HavingMySqlDatabase<BreakfastContext>()
               .WhenCreating("BreakfastItem", request)
               .ShouldReturnSuccessfulStatus()
               .JsonResultShould<BreakfastItem>(r => r.Id.Should().Be(1),
                                                r => r.Name.Should().Be(request.Name),
                                                r => r.Rating.Should().Be(request.Rating)
               .ShouldExistInDatabase<BreakfastContext, BreakfastItem>(1,
                                                x => x.Id.Should().Be(1),
                                                x => existing.Name.Should().Be(request.Name),
                                                x => x.Rating.Should().Be(request.Rating));
    }
}
```