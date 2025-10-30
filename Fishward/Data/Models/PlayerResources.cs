namespace Fishward.Data.Models;

public class PlayerResources
{
    public ulong DiscordID { get; set; }

    public int Money { get; set; }

    public virtual Player Player { get; set; } = null!;
}