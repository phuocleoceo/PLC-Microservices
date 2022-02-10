using Npgsql;
using Polly;

namespace Discount.Grpc.Extension;

public static class HostExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating Postresql Database.");

                var retry = Policy.Handle<NpgsqlException>().WaitAndRetry(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                    onRetry: (exception, retryCount, context) =>
                    {
                        logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                    });

                //if the postgresql server container is not created on run docker compose this
                //migration can't fail for network related exception. The retry options for database operations
                //apply to transient exceptions                    
                retry.Execute(() => ExecuteMigrations(configuration));

                logger.LogInformation("Migrated Postresql Database.");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the postresql database");
            }
        }
        return host;
    }

    private static void ExecuteMigrations(IConfiguration configuration)
    {
        string CNS = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        using var connection = new NpgsqlConnection(CNS);
        connection.Open();

        using var command = new NpgsqlCommand { Connection = connection };

        command.CommandText = "DROP TABLE IF EXISTS Coupon";
        command.ExecuteNonQuery();

        command.CommandText = @"CREATE TABLE Coupon(
                                    Id SERIAL PRIMARY KEY, 
                                    ProductName VARCHAR(24) NOT NULL,
                                    Description TEXT,
                                    Amount INT
                                );";
        command.ExecuteNonQuery();


        command.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount) VALUES
                                ('iPhone 6s Plus', 'My Phone Discount', 27);";
        command.ExecuteNonQuery();

        command.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount) VALUES
                                ('Dell Inspiron 5567', 'My Laptop Discount', 69);";
        command.ExecuteNonQuery();

        command.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount) VALUES
                                ('Macbook Air M1', 'Mac Discount', 10);";
        command.ExecuteNonQuery();
    }
}
