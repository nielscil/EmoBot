using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AliceWPF.Classes;
using EmotionLib.Models;

namespace AliceWPF.Converters
{
    public class BooleanToEmotionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && parameter != null)
            {
                return parameter.ToString() == value.ToString();
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            if (parameter != null)
            {
                switch (parameter.ToString())
                {
                    case "Happy":
                        return EmotionEnum.Happy;
                    case "Sad":
                        return EmotionEnum.Sad;
                    case "Scared":
                        return EmotionEnum.Fear;
                    case "Angry":
                        return EmotionEnum.Anger;
                    default:
                        return EmotionEnum.Neutral;
                }
            }
            return null;
        }
    }
}
