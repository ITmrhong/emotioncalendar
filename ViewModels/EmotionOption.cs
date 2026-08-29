using CommunityToolkit.Mvvm.ComponentModel;
using EmotionCalendarDiary.Models;

namespace EmotionCalendarDiary.ViewModels;

public partial class EmotionOption(EmotionType type) : ObservableObject
{
    public EmotionType Type { get; } = type;
    public string Label { get; } = EmotionCatalog.Label(type);
    public string Emoji { get; } = EmotionCatalog.Emoji(type);

    [ObservableProperty]
    private bool isSelected;
}

public partial class TagOption(CauseTag tag) : ObservableObject
{
    public CauseTag Tag { get; } = tag;
    public string Label { get; } = CauseTagCatalog.Label(tag);

    [ObservableProperty]
    private bool isSelected;
}
