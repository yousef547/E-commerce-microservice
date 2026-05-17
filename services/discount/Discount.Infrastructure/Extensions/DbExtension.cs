using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;


namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Discount DB Migration Started");
                    ApplyMigrations(config);
                    logger.LogInformation("Discount DB Migration Completed");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database.");
                    throw;
                }
            }
            return host;
        }

        private static void ApplyMigrations(IConfiguration config)
        {
            var retry = 5;
            while (retry > 0)
            {
                try
                {
                    //var builder = new NpgsqlConnectionStringBuilder
                    //{
                    //    Host = "localhost",
                    //    Port = 5432,
                    //    Database = "DiscountDb",
                    //    Username = "postgres",
                    //    Password = "Password",
                    //    SslMode = SslMode.Disable,
                    //    Pooling = false,
                    //    Timeout = 30,
                    //    CommandTimeout = 30
                    //};

                    using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));

                    Console.WriteLine(connection);
                    connection.Open();
                    using var cmd = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    cmd.CommandText = "DROP TABLE IF EXISTS Coupon";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                    ProductName VARCHAR(500) NOT NULL,
                                                    Description TEXT,
                                                    Amount INTEGER)";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Egypt Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500);";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('PowerFit 19 FH Rubber Spike Cricket Shoes', 'Racquet Discount', 700);";
                    cmd.ExecuteNonQuery();
                    // Exit loop if successful
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    retry--;
                    if (retry == 0)
                    {
                        throw;
                    }
                    //Wait for 2 seconds
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
