using EmotionCalendarDiary.ViewModels;

namespace EmotionCalendarDiary.Views;

public partial class DiaryEntryPage : ContentPage
{
    public DiaryEntryPage(DiaryEntryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
