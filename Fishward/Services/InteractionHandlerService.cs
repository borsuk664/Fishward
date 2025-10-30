using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Fishward.Services;

public class InteractionHandlerService(DiscordSocketClient client, InteractionService interactionService, IServiceProvider serviceProvider) : IHostedService
{
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        client.Ready += () => interactionService.RegisterCommandsGloballyAsync();
        client.InteractionCreated += OnInteractionAsync;
        await interactionService.AddModulesAsync(Assembly.GetAssembly(typeof(Program)), serviceProvider);
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        interactionService.Dispose();
        return Task.CompletedTask;
    }

    private async Task OnInteractionAsync(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(client, interaction);
            await interactionService.ExecuteCommandAsync(context, serviceProvider);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}