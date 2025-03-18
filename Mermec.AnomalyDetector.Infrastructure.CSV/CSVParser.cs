using Mermec.AnomalyDetector.Domain.Models;
using System.Text;

namespace Mermec.AnomalyDetector.Infrastructure.CSV
{
    public class CSVParser
    {
        public readonly string filePath;

        public CSVParser(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Get a collection of <see cref="Measure"/> from CSV file
        /// </summary>
        /// <returns></returns>
        public Measure[] ReadMeasureReport()
        {
            using StreamReader reader = new(filePath);

            List<Measure> result = [];

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] values = line.Split(',');

                result.Add(new Measure
                {
                    Index = int.Parse(values[0]),
                    Latitude = string.Equals(values[1], "NA", StringComparison.OrdinalIgnoreCase) ? null : new DMSPoint(values[1], PointType.Latitude),
                    Longitude = string.Equals(values[2], "NA", StringComparison.OrdinalIgnoreCase) ? null : new DMSPoint(values[2], PointType.Longitude),
                    DecayMeasure = float.Parse(values[3]),
                });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Create a CSV file with given <see cref="Anomaly"/> collection in the same folder of initial set <see cref="filePath"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string WriteReport(IEnumerable<Anomaly> value) 
        {
            string relativePath = Path.GetDirectoryName(this.filePath)!;
            string fileName = $"Report{DateTime.Now:yyyyMMddHHmmss}.csv";
            string filePath = Path.Combine(relativePath, fileName);
            string csv = GenerateCSV(value);
            File.WriteAllText(filePath, csv);
            return fileName;
        }

        /// <summary>
        /// Create a string format CSV with given <see cref="Anomaly"/> collection
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private string GenerateCSV(IEnumerable<Anomaly> collection)
        {
            if (!collection.Any())
                return "NO RESULTS";

            StringBuilder csv = new();
            foreach (Anomaly item in collection)
            {
                StringBuilder builder = new();
                
                //Indice progressivo della misura iniziale
                builder.Append($"{item.FirstMeasure.Index},");

                //Indice progressivo della misura finale
                builder.Append($"{item.LastMeasure.Index},");

                //Posizione geografica della misura iniziale (NA se non disponibile)
                builder.Append($"{item.FirstMeasure.Latitude?.ToString() ?? "NA"},{item.FirstMeasure.Longitude?.ToString() ?? "NA"},");

                //Posizione geografica della misura finale (NA se non disponibile)
                builder.Append($"{item.LastMeasure.Latitude?.ToString() ?? "NA"},{item.LastMeasure.Longitude?.ToString() ?? "NA"},");

                //Indice progressivo della misura corrispondente al valore massimo
                builder.Append($"{item.MaxMeasure.Index},");

                //Il valore più alto della misura tra tutte le misure contenute nell'anomalia
                builder.Append($"{item.MaxMeasure.DecayMeasure:N4},");

                //la lunghezza in metri del difetto
                builder.Append($"{item.AnomalyLenght:N4},");

                //la distanza in linea d'aria
                builder.Append($"{item.GeographicalLenght?.ToString("N4") ?? "NA"}");

                csv.AppendLine(builder.ToString());
            }

            return csv.ToString();
        }
    }
}
