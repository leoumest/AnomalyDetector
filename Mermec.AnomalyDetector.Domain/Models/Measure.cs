namespace Mermec.AnomalyDetector.Domain.Models
{
    public class Measure
    {
        public int Index { get; set; }
        public DMSPoint? Latitude { get; set; }
        public DMSPoint? Longitude { get; set; }
        public float DecayMeasure { get; set; }

        /// <summary>
        /// Return numeric coordinates of Latitude and Longitude only if both are not null
        /// </summary>
        /// <param name="latitude"><see cref="double"/> conversion of <see cref="DMSPoint"/></param>
        /// <param name="longitude"><see cref="double"/> conversion <see cref="DMSPoint"/></param>
        /// <returns>true if <see cref="DMSPoint"/> was converted successfully; otherwise, false.</returns>
        public bool TryGetDoubleCoordinates(out double latitude, out double longitude)
        {
            longitude = default;
            latitude = default;

            if (Latitude is null || Longitude is null)
                return false;

            longitude = Longitude.ToDouble();
            latitude = Latitude.ToDouble();
            return true;
        }

        public override string ToString()
        {
            return $"{Index} ,{Latitude?.ToString() ?? "NA"}, {Longitude?.ToString() ?? "NA"}, {DecayMeasure}";
        }
    }
}