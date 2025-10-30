using System.Text;
using Discord;
using Discord.Interactions;
using Fishward.Data.Models;
using Fishward.Services;

namespace Fishward.Modules;

[CommandContextType(InteractionContextType.Guild, InteractionContextType.BotDm, InteractionContextType.PrivateChannel)]
public class CasinoModule(PlayerProfileService playerProfileService) : InteractionModuleBase
{
    [SlashCommand("coinflip", "It's tails btw")]
    public async Task Coinflip(
        CoinSide selection,
        int amount)
    {
        
        IUser user = Context.Interaction.User;
        PlayerResources? playerResources = await playerProfileService.GetPlayerResources(user);
        if(playerResources is null) return;
        
        ComponentBuilderV2 builder = new ComponentBuilderV2();
        if (amount <= 0)
        {
            await RespondAsync("You can't gamble negative dabloons", ephemeral: true);
            return;
        }

        if (amount > playerResources.Money)
        {
            await RespondAsync("You can't gamble more than you have", ephemeral: true);
            return;
        }

        Random random = new Random();
        CoinSide result = (CoinSide)random.Next(2);

        if (selection == result)
        {
            await playerProfileService.AddPlayerMoney(user, amount);
        }
        else
        {
            await playerProfileService.AddPlayerMoney(user, -amount);
        }

        
        
        await RespondAsync(components: BuildCoinflipMessageComponent(selection, result, amount));

    }
    
    
    private MessageComponent BuildCoinflipMessageComponent(CoinSide selection, CoinSide result, int amount)
    {
        IUser user = Context.Interaction.User;
        ContainerBuilder containerBuilder = new ContainerBuilder();
        containerBuilder.WithAccentColor(Color.Blue);

        containerBuilder.WithTextDisplay($"### _{user.GlobalName}_");
        containerBuilder.WithMediaGallery([Assets.Images.GamblerPortrait]);
        containerBuilder.WithSeparator();

        containerBuilder.WithTextDisplay("## **You Bet:**");
        containerBuilder.WithTextDisplay($"# {amount} on {selection.ToString()}");
        containerBuilder.WithSeparator();
        containerBuilder.WithTextDisplay($"## **It's {result.ToString()}**");
        if (selection == result)
        {
            containerBuilder.WithTextDisplay($"## **You won {amount * 2}!**");
        }
        else
        {
            containerBuilder.WithTextDisplay($"## **You lose!**");
        }
        
        ComponentBuilderV2 componentBuilder = new ComponentBuilderV2()
            .WithContainer(containerBuilder);

        return componentBuilder.Build();
    }

    private string GetFishQualityString(int quality, int maxQuality)
    {
        string result = string.Empty;
        for (int i = 0; i < quality; i++)
        {
            result += Assets.Emotes.QualityStar;
        }

        for (int i = 0; i < maxQuality - quality; i++)
        {
            result += Assets.Emotes.QualityStarEmpty;
        }

        return result;
    }


    public enum CoinSide
    {
        Tails,
        Heads
    }
}