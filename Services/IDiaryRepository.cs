using EmotionCalendarDiary.Models;

namespace EmotionCalendarDiary.Services;

public interface IDiaryRepository
{
    Task InitializeAsync();
    Task<IReadOnlyList<DiaryEntry>> GetForMonthAsync(int year, int month);
    Task<DiaryEntry?> GetByDateAsync(DateTime date);
    Task SaveAsync(DiaryEntry entry);
    Task DeleteAsync(DiaryEntry entry);
}
