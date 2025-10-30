using Discord;
using Fishward.Assets;

namespace Fishward.Entities;

public class Fish
{
    public string Name { get; set; }
    public int Quality { get; set; }
    public int MaxQuality  { get; set; }
    public float Weight { get; set; }
    public float Length { get; set; }
    public float Calories { get; set; }
    public int Value { get; set; }
    
    public MediaGalleryItemProperties FishImage {get; set;}


    public Fish(FishData fishData, int quality)
    {
        float length = fishData.GetLength(quality);
        float weight = fishData.GetWeight(length);
        float calories = fishData.GetCalories(weight);
        int value =  fishData.GetValue(weight, quality);
        Name = fishData.Name;
        FishImage = fishData.FishImage;
        Quality = quality;
        MaxQuality = fishData.MaxQuality;
        Length = length;
        Weight = weight;
        Calories = calories;
        Value = value;
    }
    
}