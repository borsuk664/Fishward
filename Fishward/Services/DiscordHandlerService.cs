using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Fishward.Services;

public class DiscordHandlerService(DiscordSocketClient client, IOptions<Config> config) : IHostedService
{
    private readonly Config _config = config.Value;
    
    
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await client.LoginAsync(TokenType.Bot, _config.Discord.Token);
        await client.StartAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        client.Dispose();
        return Task.CompletedTask;
    }
}