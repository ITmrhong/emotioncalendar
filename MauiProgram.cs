using EmotionCalendarDiary.Services;
using EmotionCalendarDiary.ViewModels;
using EmotionCalendarDiary.Views;
using Microsoft.Extensions.Logging;

namespace EmotionCalendarDiary
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            SQLitePCL.Batteries_V2.Init();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IDiaryRepository, SqliteDiaryRepository>();
            builder.Services.AddSingleton<IPhotoService, MediaPickerPhotoService>();

            builder.Services.AddSingleton<CalendarViewModel>();
            builder.Services.AddTransient<CalendarPage>();

            builder.Services.AddTransient<DiaryEntryViewModel>();
            builder.Services.AddTransient<DiaryEntryPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
