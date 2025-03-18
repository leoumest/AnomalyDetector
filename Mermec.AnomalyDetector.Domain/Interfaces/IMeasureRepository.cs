using Mermec.AnomalyDetector.Domain.Models;

namespace Mermec.AnomalyDetector.Domain.Interfaces
{
    public interface IMeasureRepository
    {
        public Task<Measure[]> GetReport();
    }
}