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
    public class BooleanToUserEmotionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter == value ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            if (parameter != null)
            {
                switch (parameter.ToString())
                {
                    case "Happy":
                        return UserEmotion.Happy;
                    case "Sad":
                        return UserEmotion.Sad;
                    case "Scared":
                        return UserEmotion.Scared;
                    case "Angry":
                        return UserEmotion.Angry;
                    default:
                        return UserEmotion.Neutral;
                }
            }
            return null;
        }
    }
}
