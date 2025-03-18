using Mermec.AnomalyDetector.Domain.Models;

namespace Mermec.AnomalyDetector.Domain.Interfaces
{
    public interface IAnomalyRepository
    {
        public Task<string> SendReport(IEnumerable<Anomaly> dataSet);
    }
}