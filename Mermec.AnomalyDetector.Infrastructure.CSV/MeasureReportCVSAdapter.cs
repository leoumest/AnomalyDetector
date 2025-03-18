using Mermec.AnomalyDetector.Domain.Interfaces;
using Mermec.AnomalyDetector.Domain.Models;

namespace Mermec.AnomalyDetector.Infrastructure.CSV
{
    public class MeasureReportCVSAdapter : IMeasureRepository
    {
        private readonly CSVParser parser;

        public MeasureReportCVSAdapter(string filePath)
        {
            this.parser = new CSVParser(filePath);
        }

        /// <summary>
        /// Get Measure report from a CSV file with given filePath
        /// </summary>
        /// <returns></returns>
        public Task<Measure[]> GetReport()
        {
            return Task.Run(parser.ReadMeasureReport);
        }
    }
}