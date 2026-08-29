using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmotionCalendarDiary.Models;
using EmotionCalendarDiary.Services;
using EmotionCalendarDiary.Views;

namespace EmotionCalendarDiary.ViewModels;

public partial class CalendarViewModel : ObservableObject
{
    private readonly IDiaryRepository _repository;

    [ObservableProperty]
    private DateTime currentMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);

    public ObservableCollection<CalendarDayCellViewModel> Days { get; } = [];

    public CalendarViewModel(IDiaryRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadAsync()
    {
        Days.Clear();

        var firstOfMonth = CurrentMonth;
        var startOfWeek = DayOfWeek.Sunday;
        var leadingDays = ((int)firstOfMonth.DayOfWeek - (int)startOfWeek + 7) % 7;
        var gridStart = firstOfMonth.AddDays(-leadingDays);

        var entries = await _repository.GetForMonthAsync(firstOfMonth.Year, firstOfMonth.Month);
        var stampsByDate = entries.ToDictionary(e => e.Date.Date, e => EmotionCatalog.EmojiForKey(e.EmotionKey));

        for (var i = 0; i < 42; i++)
        {
            var date = gridStart.AddDays(i);
            var inCurrentMonth = date.Month == firstOfMonth.Month && date.Year == firstOfMonth.Year;

            Days.Add(new CalendarDayCellViewModel
            {
                Date = inCurrentMonth ? date : null,
                IsInCurrentMonth = inCurrentMonth,
                EmojiStamp = inCurrentMonth && stampsByDate.TryGetValue(date.Date, out var emoji) ? emoji : null
            });
        }
    }

    [RelayCommand]
    private async Task PreviousMonth()
    {
        CurrentMonth = CurrentMonth.AddMonths(-1);
        await LoadAsync();
    }

    [RelayCommand]
    private async Task NextMonth()
    {
        CurrentMonth = CurrentMonth.AddMonths(1);
        await LoadAsync();
    }

    [RelayCommand]
    private async Task SelectDay(CalendarDayCellViewModel? cell)
    {
        if (cell?.Date is not { } date)
            return;

        await Shell.Current.GoToAsync($"{nameof(DiaryEntryPage)}?date={date:yyyy-MM-dd}");
    }
}
