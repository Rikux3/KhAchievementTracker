using System;
using System.Globalization;
using System.Windows.Data;

namespace KhAchievementTracker.Application
{
    internal class GameInfoToGameTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (GameInfo)value switch
            {
                GameInfo.Kh1 => "Kingdom Hearts",
                GameInfo.Kh2 => "Kingdom Hearts II",
                GameInfo.KhRecom => "Re: Chain of Memories",
                GameInfo.KhBbs => "Birth by Sleep",
                _ => value
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                "Kingdom Hearts" => GameInfo.Kh1,
                "Kingdom Hearts II" => GameInfo.Kh2,
                "Re: Chain of Memories" => GameInfo.KhRecom,
                "Birth by Sleep" => GameInfo.KhBbs,
                _ => value
            };
        }
    }
}
