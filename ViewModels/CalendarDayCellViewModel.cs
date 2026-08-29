namespace EmotionCalendarDiary.ViewModels;

public class CalendarDayCellViewModel
{
    public DateTime? Date { get; init; }
    public bool IsInCurrentMonth { get; init; }
    public string? EmojiStamp { get; set; }
    public bool HasEntry => !string.IsNullOrEmpty(EmojiStamp);
    public int DayNumber => Date?.Day ?? 0;
}
