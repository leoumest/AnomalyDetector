using Mermec.AnomalyDetector.Domain.Interfaces;
using Mermec.AnomalyDetector.Domain.Models;

namespace Mermec.AnomalyDetector.Infrastructure.CSV
{
    public class AnomalyReportCSVAdapter : IAnomalyRepository
    {
        private readonly CSVParser parser;

        public AnomalyReportCSVAdapter(string filePath)
        {
            this.parser = new CSVParser(filePath);
        }

        /// <summary>
        /// Write the Anomaly report on a CSV file on the same folder of given filePath
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public Task<string> SendReport(IEnumerable<Anomaly> dataSet)
        {
            return Task.Run(() => {
                string filename = parser.WriteReport(dataSet);
                return filename;
            });
        }
    }
}