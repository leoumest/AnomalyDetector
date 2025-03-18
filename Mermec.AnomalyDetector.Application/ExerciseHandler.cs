using Mermec.AnomalyDetector.Domain.Interfaces;
using Mermec.AnomalyDetector.Domain.Models;
using Mermec.AnomalyDetector.Domain.Services;

namespace Mermec.AnomalyDetector.Application
{
    public class ExerciseHandler
    {
        private readonly IMeasureRepository measureReport;
        private readonly IAnomalyRepository anomalyReport;

        public ExerciseHandler(IMeasureRepository measureReport, IAnomalyRepository anomalyReport)
        {
            this.measureReport = measureReport;
            this.anomalyReport = anomalyReport;
        }

        public async Task<string> Exercise1(float thresholdValue)
        {
            Measure[] report = await measureReport.GetReport();
            IEnumerable<Anomaly> result = AnomalyFinderService.ThresholdAnomalyMeasurement(report, thresholdValue);
            return await anomalyReport.SendReport(result);
        }

        public async Task<string> Exercise2(float thresholdValue, int clusterFactor)
        {
            Measure[] report = await measureReport.GetReport();
            IEnumerable<Anomaly> result = AnomalyFinderService.ClusterAnomalyMeasurement(report, thresholdValue, clusterFactor);
            return await anomalyReport.SendReport(result);
        }

        public async Task<string> Exercise4(float thresholdValue, int clusterFactor)
        {
            Measure[] report = await measureReport.GetReport();
            IEnumerable<Anomaly> result = AnomalyFinderService.SafeDistanceAnomalyMeasurement(report, thresholdValue, clusterFactor);
            return await anomalyReport.SendReport(result);
        }

        public async Task<string> Exercise5(float thresholdValue, int clusterFactor)
        {
            Measure[] report = await measureReport.GetReport();
            IEnumerable<Anomaly> result = AnomalyFinderService.ParallelClusterAnomalyMeasurement(report, thresholdValue, clusterFactor);
            return await anomalyReport.SendReport(result);
        }
    }
}
