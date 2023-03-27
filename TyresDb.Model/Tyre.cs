using System;
using System.Text;

namespace TyresDb.Model
{
    public class Tyre
    {
        public double Diameter { get; set; }
        public double Width { get; set; }
        public double AspectRatio { get; set; }
        public double Weight { get; set; }

        public double ResultHeigh => this.CalculateTyreDiametr();

        public bool Validate(out string error)
        {
            var sb = new StringBuilder();
            
            if(Diameter == 0)
            {
                sb.Append("Отсутствует диаметр");
            }
            if (Width == 0)
            {
                sb.Append("Отсутствует ширина");
            }
            if (AspectRatio == 0)
            {
                sb.Append("Отсутствует высота профиля");
            }
            if (Weight == 0)
            {
                sb.Append("Отсутствует вес");
            }

            error = sb.ToString();

            return sb.Length == 0;
        }
        public override string ToString()
        {
            return $"{Width};{AspectRatio};{Diameter};{Weight}";
        }
    }
}
