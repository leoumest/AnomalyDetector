namespace Mermec.AnomalyDetector.Domain.Models
{
    public enum PointType
    {
        Latitude,
        Longitude
    }

    public class DMSPoint
    {
        public DMSPoint(string value, PointType pointType)
        {
            Type = pointType;
            string[] result = value.Split(' ','°', '\'', '"');

            Cardinal = result[0];
            
            //TODO: Check per convertire in numero negativo in base al punto cardinale, ma visto che è un esercizio lo lascio come ultimo task (se avanza tempo)
            Degrees = int.Parse(result[1]);
            Minutes = int.Parse(result[2]);
            Seconds = float.Parse(result[3]);
        }

        public string Cardinal { get; set; }
        public int Degrees { get; set; }
        public int Minutes { get; set; }
        public float Seconds { get; set; }
        public PointType Type { get; set; }
        
        public double ToDouble()
        {
            return (double)Degrees + ((double)Minutes)/60 + ((double)Seconds)/3600;
        }

        public override string ToString()
        {
            return $"{Cardinal} {Degrees}°{Minutes}'{Seconds:N4}\"";
        }
    }
}