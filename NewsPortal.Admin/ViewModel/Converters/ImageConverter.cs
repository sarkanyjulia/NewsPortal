﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NewsPortal.Admin.ViewModel
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Byte[]))
                return Binding.DoNothing;

            try
            {
                using (MemoryStream stream = new MemoryStream(value as Byte[])) // a képet a memóriába egy adatfolyamba helyezzük
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // a betöltött tartalom a képbe kerül
                    image.StreamSource = stream; // átalakítjuk bitképpé
                    image.EndInit();
                    return image; // visszaadjuk a létrehozott bitképet
                }
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
