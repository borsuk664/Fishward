using Discord;
using Fishward.Entities;

namespace Fishward.Assets;

public static class FishDatabase
{
    public static readonly FishData Fish1 = new FishData()
    {
        Name = "Small Chickenfish",
        BaseDifficulty = 1000,
        BaseQualityDifficulty = 100,
        FishImage = Assets.Images.SmallChickenFish
    };
    
    public static readonly FishData Fish2 = new FishData()
    {
        Name = "Small Mouthbass",
        BaseDifficulty = 2000,
        BaseQualityDifficulty = 100,
        FishImage = Assets.Images.SmallMouthBass
    };
    
    
    public static readonly FishData Fish3 = new FishData()
    {
        Name = "Small Slimetail",
        BaseDifficulty = 4700,
        BaseQualityDifficulty = 100,
        FishImage = Assets.Images.SmallSlimeTail
    };
    
    public static readonly FishData Fish4 = new FishData()
    {
        Name = "fish4",
        BaseDifficulty = 20000,
        BaseQualityDifficulty = 100,
    };
    
    public static readonly HashSet<FishData> EnabledFishes = [Fish1, Fish2, Fish3, Fish4];

    
}

