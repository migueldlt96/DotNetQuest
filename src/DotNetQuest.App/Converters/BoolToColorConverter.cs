using System.Globalization;

namespace DotNetQuest.App.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCurrent && isCurrent)
        {
            return Color.FromArgb("#6a4a8a"); // Current realm - purple
        }
        return Color.FromArgb("#3a2a4a"); // Other realms - darker
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
