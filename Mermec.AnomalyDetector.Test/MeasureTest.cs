using Mermec.AnomalyDetector.Domain.Models;

namespace Mermec.AnomalyDetector.Test
{
    public class MeasureTest
    {

        [Fact]
        public void TryGetDoubleCoordinatesTest_ShouldFail()
        {
            //Arrange
            Measure measure = new();

            //Act
            bool result = measure.TryGetDoubleCoordinates(out double latitudeResult, out double longitudeResult);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetDoubleCoordinatesTest_ShouldMatch()
        {
            //Arrange
            Measure measure = new()
            {
                Latitude = new DMSPoint("N 45°29'12.0035\"", PointType.Latitude),
                Longitude = new DMSPoint("E 9°12'18.9851\"", PointType.Longitude)
            };

            double expectedLatitude = 45.48666763888889d;
            double expectedLongitude = 9.205273638888888d;

            //Act
            bool result = measure.TryGetDoubleCoordinates(out double latitudeResult, out double longitudeResult);
            
            //Assert
            Assert.True(result);
            Assert.Equal(expectedLatitude.ToString("N6"), latitudeResult.ToString("N6"));
            Assert.Equal(expectedLongitude.ToString("N6"), longitudeResult.ToString("N6"));
        }      
    }
}