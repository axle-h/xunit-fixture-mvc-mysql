using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Xunit.Fixture.Mvc.Extensions;

namespace Xunit.Fixture.Mvc.MySql.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IMvcFunctionalTestFixture"/> for configuring a MySQL database.
    /// </summary>
    public static class MySqlMvcFunctionalTestFixtureArrangeExtensions
    {
        /// <summary>
        /// Configures a MySQL database with the specified context type.
        /// This will create a bootstrap job that will drop and recreate the MySQL database.
        /// The connection string is modified to create a database named after the current executing test.
        /// I.e. tests can run in parallel.
        /// For this to work, the connection string must have root permissions i.e. be able to drop and recreate any database.
        /// We assume that this super user connection string can be found in the FunctionalTests environment.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="bootstrap">The bootstrap.</param>
        /// <param name="environment">The environment.</param>
        /// <returns></returns>
        public static IMvcFunctionalTestFixture HavingMySqlDatabase<TDbContext>(this IMvcFunctionalTestFixture fixture,
                                                                                Action<IMySqlBootstrapDataContext> bootstrap = null,
                                                                                string environment = "FunctionalTests")
            where TDbContext : DbContext
        {
            var testData = new MySqlBootstrapDataContext();
            bootstrap?.Invoke(testData);

            return fixture.HavingAspNetEnvironment(environment)
                          .HavingBootstrap<MySqlBootstrap<TDbContext>>()
                          .HavingServices(services => services.AddSingleton(testData))
                          .HavingConfiguration((output, builder) =>
                                               {
                                                   var testName = output.GetCurrentTestName();

                                                   // We cannot use the test name directly for the database name as it may be longer than the maximum allowed 64 characters.
                                                   var testNameHash = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(testName)));
                                                   output.WriteLine("Using test database: " + testNameHash);

                                                   var connectionString = new MySqlConnectionStringBuilder(builder.Build().GetConnectionString("mysql"))
                                                                          {
                                                                              Database = testNameHash.ToString()
                                                                          }.ToString();
                                                   var key = ConfigurationPath.Combine("ConnectionStrings", "mysql");
                                                   var memoryConfig = new Dictionary<string, string> {[key] = connectionString};
                                                   builder.AddInMemoryCollection(memoryConfig);
                                               });
        }
    }
}
