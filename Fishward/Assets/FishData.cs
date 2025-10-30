using Discord;
using Fishward.Entities;

namespace Fishward.Assets;

public class FishData
{
    public required string Name { get; init;}
    public FishingLocation  Location { get; init;}
    public required int BaseDifficulty { get; init;}
    public int MaxQuality { get; init; } = 10;
    public required int BaseQualityDifficulty { get; init; }
    public float QualityDifficultyScaling { get; init; } = 2;
    public float BaseLength { get; init; } = 3;
    public float LengthVariance { get; init; } = 0.2f;
    public float QualityLengthScaling { get; init; } = 1.1f;
    public float WeightDensity { get; init; } = 0.02f;
    public float WeightVariance { get; init; } = 0.2f;
    public float CaloryDensity { get; init; } = 2200f;
    public float ValuePerWeight { get; init; } = 100f;
    public float QualityValueScaling { get; init; } = 2f;
    public MediaGalleryItemProperties FishImage { get; init; }

    
    
    public float GetLength(int quality)
    {
        float length = BaseLength;
        length *= GetQualityMultiplier(quality, QualityLengthScaling);
        length *= GetRandomVarianceValue(LengthVariance);
        return float.Round(length, 2);
    }

    public float GetWeight(float length)
    {
        float weight = length * WeightDensity;
        weight *= GetRandomVarianceValue(WeightVariance);
        return float.Round(weight, 2);;
    }

    public float GetCalories(float weight)
    {
        float calories = CaloryDensity * weight;
        return float.Round(calories, 2);;
    }

    
    public int GetValue(float weight, int quality)
    {
        float value = ValuePerWeight * weight;
        value *= GetQualityMultiplier(quality, QualityValueScaling);
        return (int)value;
    }

    private float GetQualityMultiplier(int quality, float growthFactor)
    {
        float multiplier = 1;
        for (int i = 0; i < quality-1; i++)
        {
            multiplier *= growthFactor;
        }

        return multiplier;
    }
    
    public float GetRandomVarianceValue(float variance)
    {
        return 1 + Random.Shared.NextSingle() * variance * 2 - variance;
    }
}