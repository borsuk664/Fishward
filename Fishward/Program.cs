using System.Globalization;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Fishward.Data;
using Fishward.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fishward;

public class Program
{

    private static void CreateServices(IHostBuilder hostBuilder)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = configurationBuilder.Build();
        var databaseConfig = new Config.DatabaseConfig();
        configuration.GetSection("Database").Bind(databaseConfig);

        var discordConfig = new DiscordSocketConfig()
        {
            
        };
        DiscordSocketClient discordSocketClient = new DiscordSocketClient(discordConfig);

        hostBuilder.ConfigureServices(serviceCollection =>
        {
            serviceCollection
                .AddSingleton(discordSocketClient)
                .AddSingleton(new InteractionService(discordSocketClient))
                .AddHostedService<DiscordHandlerService>()
                .AddHostedService<InteractionHandlerService>()
                .AddSingleton<PlayerProfileService>()
                .AddSingleton<FishingService>()
                .AddDbContext<AppDbContext>(options => { options.UseNpgsql(databaseConfig.GetConnectionString()); })
                .AddLogging()
                .AddHostedService<LoggingService>()
                .Configure<Config>(configuration);
        });
    }

    private static async Task MigrateDatabase(IHost host)
    {
        using var serviceScope = host.Services.GetService<IServiceScopeFactory>()?.CreateScope();
        if (serviceScope is null) throw new Exception("Service Scope is null");
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        var fish = serviceScope.ServiceProvider.GetRequiredService<FishingService>();
        fish.StartAsync(new CancellationToken());
        await context.Database.MigrateAsync();
    }

    private static async Task Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        IHostBuilder builder = Host.CreateDefaultBuilder(args);
        CreateServices(builder);
        IHost host = builder.Build();
        await MigrateDatabase(host);

        await host.RunAsync();
    }
}