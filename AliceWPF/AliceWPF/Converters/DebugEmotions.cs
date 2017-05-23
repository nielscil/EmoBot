using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AliceWPF.Classes;

namespace AliceWPF.Converters
{
    public class DebugEmotions : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            if (value.ToString() == "Happy")
            {
                return UserEmotion.Happy;
            }
            else if (value.ToString() == "Sad")
            {
                return UserEmotion.Sad;
            }
            else if (value.ToString() == "Scared")
            {
                return UserEmotion.Scared;
            }
            else if (value.ToString() == "Angry")
            {
                return UserEmotion.Angry;
            }
            return UserEmotion.Neutral;
        }
    }
}
