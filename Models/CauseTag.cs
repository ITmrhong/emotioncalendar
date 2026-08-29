namespace EmotionCalendarDiary.Models;

public enum CauseTag
{
    Work,
    Love,
    People,
    Health
}

public static class CauseTagCatalog
{
    public static readonly IReadOnlyList<CauseTag> All =
    [
        CauseTag.Work,
        CauseTag.Love,
        CauseTag.People,
        CauseTag.Health
    ];

    public static string Label(CauseTag tag) => tag switch
    {
        CauseTag.Work => "#업무",
        CauseTag.Love => "#연애",
        CauseTag.People => "#사람",
        CauseTag.Health => "#건강",
        _ => tag.ToString()
    };

    public static string ToStorageString(IEnumerable<CauseTag> tags) =>
        string.Join(",", tags.Select(t => t.ToString()));

    public static IReadOnlyList<CauseTag> FromStorageString(string? stored)
    {
        if (string.IsNullOrWhiteSpace(stored))
            return [];

        return stored
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => Enum.TryParse<CauseTag>(s, out var tag) ? tag : (CauseTag?)null)
            .Where(t => t.HasValue)
            .Select(t => t!.Value)
            .ToList();
    }
}
