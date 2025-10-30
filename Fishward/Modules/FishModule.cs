using Discord;
using Discord.Interactions;
using Fishward.Entities;
using Fishward.Services;

namespace Fishward.Modules;

[CommandContextType(InteractionContextType.Guild, InteractionContextType.BotDm, InteractionContextType.PrivateChannel)]
public class FishModule(PlayerProfileService playerProfileService, FishingService fishingService) : InteractionModuleBase
{
    [SlashCommand("fish", "fishes")]
    public async Task Fish()
    {
        IUser user = Context.Interaction.User;

        
        Fish fish = fishingService.CatchFish(user);
        await playerProfileService.AddPlayerMoney(user, fish.Value);
        
        
        MessageComponent message = BuildFishMessageComponent(fish);
        
        await RespondAsync(components: message);

    }

    private MessageComponent BuildFishMessageComponent(Fish caughtFish)
    {
        IUser user = Context.Interaction.User;
        ContainerBuilder containerBuilder = new ContainerBuilder();
        containerBuilder.WithAccentColor(Color.Blue);

        containerBuilder.WithTextDisplay($"### _{user.GlobalName}_");
        // containerBuilder.WithMediaGallery([Assets.Images.FishingBanner]);
        containerBuilder.WithSeparator();

        containerBuilder.WithTextDisplay("**You caught:**");
        containerBuilder.WithTextDisplay($"# {caughtFish.Name}");
        containerBuilder.WithTextDisplay(GetFishQualityString(caughtFish.Quality, caughtFish.MaxQuality));
        containerBuilder.WithMediaGallery([caughtFish.FishImage]);
        containerBuilder.WithSeparator();


        string fishWeight = $"{Assets.Emotes.Weight}{caughtFish.Weight}";
        string fishLength = $"{Assets.Emotes.Length}{caughtFish.Length}cm";
        // Alignment has to be calculated without emotes as their string representation is different size than when displayed as emote 
        string fishCalories = $"{GetAlignmentOffset($"{caughtFish.Weight}", 35)}{Assets.Emotes.Calories}{caughtFish.Calories}cal";
        string fishValue = $"{GetAlignmentOffset($"{caughtFish.Length}cm", 35)}{Assets.Emotes.Money}{caughtFish.Value}";
        containerBuilder.WithTextDisplay($"### {fishWeight} {fishCalories}");
        containerBuilder.WithTextDisplay($"### {fishLength} {fishValue}");
        
        ButtonBuilder fishAgainButton = new ButtonBuilder()
            .WithLabel($"Fish Again")
            .WithCustomId("fishAgain")
            .WithStyle(ButtonStyle.Primary);


        ComponentBuilderV2 componentBuilder = new ComponentBuilderV2()
            .WithContainer(containerBuilder)
            .WithActionRow([fishAgainButton]);

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

    // TODO: Should take letter size into account for more accurate alignment
    private string GetAlignmentOffset(string currentText, int desiredOffset)
    {
        string offset = string.Empty;
        for (int i = 0; i < desiredOffset - currentText.Length; i++)
        {
            offset += " ";
        }

        return offset;
    }



    [ComponentInteraction("fishAgain")]
    public async Task FishAgain()
    {
        await Fish();
    }
}