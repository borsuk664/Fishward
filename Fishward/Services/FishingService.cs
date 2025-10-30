using System.Collections.Immutable;
using Discord;
using Fishward.Assets;
using Fishward.Entities;
using Microsoft.Extensions.Hosting;
using ScottPlot;
using static Fishward.Assets.FishDatabase;

namespace Fishward.Services;

public class FishingService : IHostedService
{
    private readonly Dictionary<FishingLocation, ImmutableSortedDictionary<int, FishData>> _fishDifficultyByLocation = LoadFishes();
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }



    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    


    public Fish CatchFish(IUser user)
    {
        
        //TODO: Fetch user fishing power
        int fishingPower = 5000;

        //TODO: Fetch user fishing location
        FishData fishData = GetRandomFish(fishingPower, FishingLocation.Lake);
        
        int quality = RollQuality(fishData, fishingPower);
        return new Fish(fishData, quality);
    }

    
    
    private FishData GetRandomFish(int fishingPower, FishingLocation location)
    {
        List<FishData> catchPool = GetPossibleCatches(FishingLocation.Lake, fishingPower);
        List<float> adjustedFishWeights = GetAdjustedFishWeights(catchPool, fishingPower);
        float total = adjustedFishWeights.Sum();
        float randomRoll = Random.Shared.NextSingle() * total;

        FishData selectedFish = null!;
        double cumulative = 0;

        for (int i = 0; i < adjustedFishWeights.Count; i++)
        {
            cumulative += adjustedFishWeights[i];
            if (!(randomRoll <= cumulative)) continue;
            selectedFish = catchPool[i];
            break;
        }

        return selectedFish;
    }
    
    private int RollQuality(FishData fish, int fishingPower)
    {
        int currentQuality = 1;
        int randomRoll = Random.Shared.Next(fishingPower);
        float qualityThreshold = fish.BaseDifficulty * fish.QualityDifficultyScaling;
        while (randomRoll > qualityThreshold && currentQuality < fish.MaxQuality)
        {
            currentQuality++;
            qualityThreshold *= fish.QualityDifficultyScaling;
        }
        
        return currentQuality;
    }
    
    private List<FishData> GetPossibleCatches(FishingLocation location, int fishingPower)
    {
        List<FishData> possibleCatches = [];
        foreach (var (difficulty, fishData) in _fishDifficultyByLocation[location])
        {
            if (fishingPower >= difficulty)
                possibleCatches.Add(fishData);
            else
                break;
        }

        return possibleCatches;
    }

    private List<float> GetAdjustedFishWeights(List<FishData> fishPool, int fishingPower)
    {
        List<float> adjustedWeighting = new List<float>();
        foreach (var fish in fishPool)
        {
            float difference = fish.BaseDifficulty / (float)fishingPower;
            float distancePenalty = (float)fishingPower/ (fishingPower - fish.BaseDifficulty);
            adjustedWeighting.Add(MathF.Pow(difference/distancePenalty, 1.1f));
        }

        return adjustedWeighting;
    }

    
    
    private static Dictionary<FishingLocation, ImmutableSortedDictionary<int, FishData>> LoadFishes()
    {
        Dictionary<FishingLocation, OrderedDictionary<int, FishData>> fishingLocations = new();
        foreach (var fishingLocation in Enum.GetValues<FishingLocation>())
        {
            fishingLocations.Add(fishingLocation, new OrderedDictionary<int, FishData>());
        }
        
        foreach (var fishData in EnabledFishes)
        {
            fishingLocations[fishData.Location].Add(fishData.BaseDifficulty,  fishData);
        }
        
        Dictionary<FishingLocation, ImmutableSortedDictionary<int, FishData>> sortedFishingLocations = new();
        foreach (var (location, weightings) in fishingLocations)
        {
            sortedFishingLocations[location] = weightings.ToImmutableSortedDictionary();
        }

        return sortedFishingLocations;
    }

}