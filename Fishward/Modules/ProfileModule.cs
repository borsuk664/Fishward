using Discord;
using Discord.Interactions;
using Fishward.Data.Models;
using Fishward.Services;

namespace Fishward.Modules;

[CommandContextType(InteractionContextType.Guild, InteractionContextType.BotDm, InteractionContextType.PrivateChannel)]
public class ProfileModule(PlayerProfileService playerProfileService) : InteractionModuleBase
{

    [SlashCommand("profile", "Displays your profile")]
    public async Task ShowProfile()
    {
        IUser user = Context.Interaction.User;
        PlayerResources? playerResources = await playerProfileService.GetPlayerResources(user);
        if(playerResources is null) return;
        
        ContainerBuilder mainContainer = new ContainerBuilder();
        mainContainer.WithTextDisplay($"Money: {playerResources.Money}");
        mainContainer.WithAccentColor(Color.Red);

        ComponentBuilderV2 componentBuilder = new ComponentBuilderV2();
        componentBuilder.WithContainer(mainContainer);
        
        await RespondAsync(components: componentBuilder.Build());
    }
    
}