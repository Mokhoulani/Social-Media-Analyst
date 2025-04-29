namespace Application.Common.CustomAttributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CacheAttribute(string cacheKeyTemplate, int durationInSeconds = 300) : Attribute
{
    public string CacheKeyTemplate { get; } = cacheKeyTemplate;
    public int DurationInSeconds { get; } = durationInSeconds;
    
}