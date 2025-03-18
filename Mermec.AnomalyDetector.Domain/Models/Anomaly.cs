namespace Mermec.AnomalyDetector.Domain.Models
{
    public class Anomaly
    {
        public required Measure FirstMeasure { get; set; }
        public required Measure LastMeasure { get; set; }
        public required Measure MaxMeasure { get; set; }
        public double AnomalyLenght { get; set; }
        public double? GeographicalLenght { get; set; } 
    }
}