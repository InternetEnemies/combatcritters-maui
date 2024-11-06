using System.Globalization;
namespace Combat_Critters_2._0.Converters
{
    public class ImageUrlConverter : IValueConverter
    {
        private const string MediaRoot = "https://combatcritters.s3.us-east-1.amazonaws.com";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrEmpty(imagePath))
            {
                return $"{MediaRoot}/{imagePath}";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported in ImageUrlConverter.");
        }
    }
}