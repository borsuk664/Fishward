using System.Text;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace Fishward.Services;

public class LoggingService : IHostedService
{
    public LoggingService(DiscordSocketClient client, InteractionService interaction)
    {
        client.Log += LogAsync;
        interaction.Log += LogAsync;
    }
    private Task LogAsync(LogMessage message)
    {
        Console.WriteLine(message.ToString());

        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}