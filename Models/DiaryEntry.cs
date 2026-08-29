using SQLite;

namespace EmotionCalendarDiary.Models;

[Table("DiaryEntries")]
public class DiaryEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Unique = true)]
    public DateTime Date { get; set; }

    public string EmotionKey { get; set; } = EmotionType.Neutral.ToString();

    public string Body { get; set; } = string.Empty;

    public string? PhotoPath { get; set; }

    public string Tags { get; set; } = string.Empty;

    public bool IsPublic { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
