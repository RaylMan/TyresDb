using System.Text;
using TyresDb.Model.Properties;

namespace TyresDb.Model
{
    public class TyresCsvRepository : ITyresRepository
    {
        private readonly string csvFilePath;
        private List<Tyre> tyres = new List<Tyre>();

        public List<Tyre> Tyres => tyres;

        public TyresCsvRepository()
        {
            this.csvFilePath = Resources.DbFilePath;
        }

        public bool Load(out string errorText)
        {
            var errors = new StringBuilder();
            try
            {
                using var reader = new StreamReader(csvFilePath);

                int row = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        row++;
                        continue;
                    }

                    var values = line.Split(';');

                    var tyre = new Tyre()
                    {
                        Width = values.TryGetDoubleValue(0),
                        AspectRatio = values.TryGetDoubleValue(1),
                        Diameter = values.TryGetDoubleValue(2),
                        Weight = values.TryGetDoubleValue(3),
                        Name = values.TryGetStringValue(4),
                        Season = values.TryGetStringValue(5)
                    };

                    if (!tyre.Validate(out var error))
                    {
                        errors.AppendLine($"Строка {row}. Текст: {line}. Ошибка: {error}.");
                    }
                    tyres.Add(tyre);
                    row++;
                }
            }
            catch (IOException)
            {
                errors.AppendLine("Ошибка открытия файла Tyres.csv файл отсутствует или открыт в другой программе");
            }

            errorText = errors.ToString();
            return errors.Length == 0;
        }

        public void Save()
        {
            var sb = new StringBuilder();

            foreach (var tyre in Tyres)
            {
                sb.AppendLine(tyre.ToString());
            }

            try
            {
                using var writer = new StreamWriter(csvFilePath, false);
                writer.Write(sb);
            }
            catch (IOException)
            {
                throw new Exception("Закройте файл Tyres.csv");
            }
        }
    }
}
