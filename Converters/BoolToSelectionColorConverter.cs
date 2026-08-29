using System.Globalization;

namespace EmotionCalendarDiary.Converters;

public class BoolToSelectionColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true ? Colors.MediumPurple : Colors.LightGray;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
