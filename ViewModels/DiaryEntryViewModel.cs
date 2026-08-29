using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmotionCalendarDiary.Models;
using EmotionCalendarDiary.Services;

namespace EmotionCalendarDiary.ViewModels;

[QueryProperty(nameof(DateText), "date")]
public partial class DiaryEntryViewModel : ObservableObject
{
    private readonly IDiaryRepository _repository;
    private readonly IPhotoService _photoService;

    private DiaryEntry? _existingEntry;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private string body = string.Empty;

    [ObservableProperty]
    private string? photoPath;

    [ObservableProperty]
    private bool isPublic;

    [ObservableProperty]
    private bool isExistingEntry;

    public string DateText
    {
        set
        {
            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                SelectedDate = date;
                _ = LoadAsync();
            }
        }
    }

    public ObservableCollection<EmotionOption> Emotions { get; } = new(EmotionCatalog.All.Select(t => new EmotionOption(t)));
    public ObservableCollection<TagOption> Tags { get; } = new(CauseTagCatalog.All.Select(t => new TagOption(t)));

    public bool CanCapturePhoto => _photoService.IsCaptureSupported;

    public DiaryEntryViewModel(IDiaryRepository repository, IPhotoService photoService)
    {
        _repository = repository;
        _photoService = photoService;
    }

    private async Task LoadAsync()
    {
        foreach (var e in Emotions) e.IsSelected = false;
        foreach (var t in Tags) t.IsSelected = false;
        Body = string.Empty;
        PhotoPath = null;
        IsPublic = false;

        _existingEntry = await _repository.GetByDateAsync(SelectedDate);
        IsExistingEntry = _existingEntry is not null;

        if (_existingEntry is null)
            return;

        Body = _existingEntry.Body;
        PhotoPath = _existingEntry.PhotoPath;
        IsPublic = _existingEntry.IsPublic;

        if (Enum.TryParse<EmotionType>(_existingEntry.EmotionKey, out var emotionType))
        {
            var match = Emotions.FirstOrDefault(e => e.Type == emotionType);
            if (match is not null)
                match.IsSelected = true;
        }

        foreach (var tag in CauseTagCatalog.FromStorageString(_existingEntry.Tags))
        {
            var match = Tags.FirstOrDefault(t => t.Tag == tag);
            if (match is not null)
                match.IsSelected = true;
        }
    }

    [RelayCommand]
    private void SelectEmotion(EmotionOption? option)
    {
        if (option is null)
            return;

        foreach (var e in Emotions)
            e.IsSelected = e == option;
    }

    [RelayCommand]
    private void ToggleTag(TagOption? option)
    {
        if (option is null)
            return;

        option.IsSelected = !option.IsSelected;
    }

    [RelayCommand]
    private async Task PickPhoto()
    {
        var path = await _photoService.PickPhotoAsync();
        if (path is not null)
            PhotoPath = path;
    }

    [RelayCommand]
    private async Task CapturePhoto()
    {
        var path = await _photoService.CapturePhotoAsync();
        if (path is not null)
            PhotoPath = path;
    }

    [RelayCommand]
    private async Task Save()
    {
        var selectedEmotion = Emotions.FirstOrDefault(e => e.IsSelected);
        if (selectedEmotion is null)
        {
            await Shell.Current.DisplayAlert("알림", "감정을 선택해주세요.", "확인");
            return;
        }

        var entry = _existingEntry ?? new DiaryEntry { Date = SelectedDate };
        entry.EmotionKey = selectedEmotion.Type.ToString();
        entry.Body = Body;
        entry.PhotoPath = PhotoPath;
        entry.IsPublic = IsPublic;
        entry.Tags = CauseTagCatalog.ToStorageString(Tags.Where(t => t.IsSelected).Select(t => t.Tag));

        await _repository.SaveAsync(entry);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (_existingEntry is null)
            return;

        await _repository.DeleteAsync(_existingEntry);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
