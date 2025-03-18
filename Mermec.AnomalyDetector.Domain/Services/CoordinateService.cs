namespace Mermec.AnomalyDetector.Domain.Services
{
    public class CoordinateService
    {
        private const double earthRadiusInMetersAtSeaLevel = 6378137.0;
        private const double earthRadiusInMetersAtPole = 6356752.314;

        //[Obsolete]
        //public static double GetDistanceInMeters(double originLongitude, double originLatitude, double destinationLongitude, double destinationLatitude)
        //{
        //    double d1 = originLatitude * (Math.PI / 180.0);
        //    double num1 = originLongitude * (Math.PI / 180.0);
        //    double d2 = destinationLatitude * (Math.PI / 180.0);
        //    double num2 = destinationLongitude * (Math.PI / 180.0) - num1;
        //    double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
        //    return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        //}

        //[Obsolete]
        //public static double GetDistanceInMetersV2(double originLongitude, double originLatitude, double destinationLongitude, double destinationLatitude)
        //{
        //    double d1 = originLatitude * (Math.PI / 180.0);
        //    double num1 = originLongitude * (Math.PI / 180.0);
        //    double d2 = destinationLatitude * (Math.PI / 180.0);
        //    double num2 = destinationLongitude * (Math.PI / 180.0) - num1;
        //    double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
        //    return 6371000.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        //}

        /// <summary>
        /// Calculate geographic distance between 2 point using Haversine formula 
        /// </summary>
        /// <param name="originLongitude"></param>
        /// <param name="originLatitude"></param>
        /// <param name="destinationLongitude"></param>
        /// <param name="destinationLatitude"></param>
        /// <returns></returns>
        public static double GetDistanceInMetersV3(double originLongitude, double originLatitude, double destinationLongitude, double destinationLatitude)
        {
            double d1 = originLatitude * (Math.PI / 180.0);
            double num1 = originLongitude * (Math.PI / 180.0);
            double d2 = destinationLatitude * (Math.PI / 180.0);
            double num2 = destinationLongitude * (Math.PI / 180.0) - num1;
            double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            double earthRadius = GetEarthRadiusByLatitude(originLongitude);
            return earthRadius * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        /// <summary>
        /// Calculate Earth radius using Geodetic Latitude
        /// </summary>
        /// <param name="originLongitude"></param>
        /// <returns></returns>
        private static double GetEarthRadiusByLatitude(double originLongitude)
        {
            double lat = originLongitude * (Math.PI / 180);
            double f1 = Math.Pow((Math.Pow(earthRadiusInMetersAtSeaLevel, 2) * Math.Cos(lat)), 2);
            double f2 = Math.Pow((Math.Pow(earthRadiusInMetersAtPole, 2) * Math.Sin(lat)), 2);
            double f3 = Math.Pow((earthRadiusInMetersAtSeaLevel * Math.Cos(lat)), 2);
            double f4 = Math.Pow((earthRadiusInMetersAtPole * Math.Sin(lat)), 2);

            return Math.Sqrt((f1 + f2) / (f3 + f4));
        }
    }
}