using Discord;
using Fishward.Data;
using Fishward.Data.Models;
using Microsoft.Extensions.Hosting;

namespace Fishward.Services;

public class PlayerProfileService(AppDbContext appDbContext)
{

    public async Task<Player?> CreatePlayerProfile(IUser user)
    {
        try
        {
            Player playerProfile = new()
            {
                DiscordID = user.Id,
                PlayerResource = new PlayerResources()
                {
                    Money = 10000
                }
            };
            
            appDbContext.Players.Add(playerProfile);
            await appDbContext.SaveChangesAsync();
            return playerProfile;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    

    public async Task AddPlayerMoney(IUser user, int amount)
    {
        PlayerResources? playerResources = await appDbContext.PlayerResources.FindAsync(user.Id);
        if(playerResources is null) return;
        playerResources.Money += amount;
        await appDbContext.SaveChangesAsync();
    }

    public async Task<PlayerResources?> GetPlayerResources(IUser user)
    {
        PlayerResources? playerResources = await appDbContext.PlayerResources.FindAsync(user.Id);
        if (playerResources is not null) return playerResources;
        Player? player = await appDbContext.Players.FindAsync(user.Id);
        if (player is not null) return null;
        
        player = await CreatePlayerProfile(user);
        if(player is not null)
            return await GetPlayerResources(user);
        return null;
    }
}