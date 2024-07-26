using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Maui.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Helpers.Converters
{
    public class GameConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string results)
            {
                var formattedText = new FormattedString();

                foreach (char result in results)
                {
                    var color = GetResultColor(result);

                    formattedText.Spans.Add(new Span { Text = result.ToString(), TextColor = color });
                }

                return formattedText;
            }

            return value; // Return original value if not a string
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Color GetResultColor(char result)
        {
            if (result == 'W')
                return Color.FromRgb(0, 128, 0);
            else if (result == 'L')
                return Color.FromRgb(255, 0, 0);
            else
                return Colors.Black; // Default color
        }
    }
}
