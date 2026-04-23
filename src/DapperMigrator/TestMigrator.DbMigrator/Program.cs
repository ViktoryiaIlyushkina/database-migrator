using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestMigrator.DbMigrator.Migrations;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) 
    .AddEnvironmentVariables() 
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

var serviceProvider = CreateServices(connectionString!);

using (var scope = serviceProvider.CreateScope())
{
    UpdateDatabase(scope.ServiceProvider);
}

static IServiceProvider CreateServices(string connectionString)
{
    return new ServiceCollection()
        .AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres() 
            .WithGlobalConnectionString(connectionString)
            .ScanIn(typeof(InitialMigration).Assembly).For.Migrations()) 
        .AddLogging(lb => lb.AddFluentMigratorConsole())
        .BuildServiceProvider(false);
}

static void UpdateDatabase(IServiceProvider serviceProvider)
{
    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

    runner.MigrateUp();
}
