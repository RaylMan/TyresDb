using System.Collections.ObjectModel;

namespace TyresDb.Model
{
    public static class TyreHelpers
    {
        /// <summary>
        /// Расчет диаметра в мм
        /// </summary>
        /// <param name="r">Диаметр в люймах</param>
        /// <param name="width">Ширина</param>
        /// <param name="aspectRatio">Профиль шины</param>
        public static double CalculateTyreDiametr(double r, double width, double aspectRatio)
        {
            double coefficient = 2.54;
            var r1 = (r * coefficient) * 10;
            var r2 = ((width / 100 * aspectRatio) * 2);
            return Math.Round(r1 + r2);
        }

        /// <summary>
        /// Расчет диаметра в мм
        /// </summary>
        public static double CalculateTyreDiametr(this Tyre tyre)
        {
            double coefficient = 2.54;
            var r1 = (tyre.Diameter * coefficient) * 10;
            var r2 = ((tyre.Width / 100 * tyre.AspectRatio) * 2);
            return Math.Round(r1 + r2);
        }

        public static double GetDoubleFromString(this string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return default(double);

                var v = value.Replace('.', ',');
                return double.Parse(v);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static double TryGetDoubleValue(this string[] values, int index)
        {
            try
            {
                var value = values[index].Replace('.', ',');
                return double.Parse(value);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static string TryGetStringValue(this string[] values, int index)
        {
            try
            {
                return values[index];
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static ObservableCollection<Tyre> ToObservableCollection(this IEnumerable<Tyre> tyres)
        {
            return new ObservableCollection<Tyre>(tyres);
        }
    }
}
