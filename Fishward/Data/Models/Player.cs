namespace Fishward.Data.Models;

public class Player
{
    public ulong DiscordID { get; set; }

    public virtual required PlayerResources PlayerResource { get; set; } = null!;
}