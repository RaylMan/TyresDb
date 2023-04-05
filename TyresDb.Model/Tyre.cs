using System.Text;

namespace TyresDb.Model
{
    public class Tyre
    {
        /// <summary>
        /// Диаметр
        /// </summary>
        public double Diameter { get; set; }
        /// <summary>
        /// Ширина
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Высота профиля
        /// </summary>
        public double AspectRatio { get; set; }
        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Сезон
        /// </summary>
        public string Season { get; set; }

        public double ResultHeigh => this.CalculateTyreDiametr();

        public bool Validate(out string error)
        {
            var sb = new StringBuilder();

            if (Diameter == 0)
            {
                sb.AppendLine("Отсутствует диаметр.");
            }
            if (Width == 0)
            {
                sb.AppendLine("Отсутствует ширина.");
            }
            if (AspectRatio == 0)
            {
                sb.AppendLine("Отсутствует высота профиля.");
            }
            if (Weight == 0)
            {
                sb.AppendLine("Отсутствует вес.");
            }

            error = sb.ToString();

            return sb.Length == 0;
        }
        public override string ToString()
        {
            return $"{Width};{AspectRatio};{Diameter};{Weight};{Name};{Season}";
        }

        public Tyre Clone() =>
             new Tyre()
             {
                 Diameter = this.Diameter,
                 Width = this.Width,
                 AspectRatio = this.AspectRatio,
                 Weight = this.Weight,
                 Name = this.Name,
                 Season = this.Season
             };

    }
}
