using EmotionCalendarDiary.Views;

namespace EmotionCalendarDiary
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DiaryEntryPage), typeof(DiaryEntryPage));
        }
    }
}
