using Mermec.AnomalyDetector.Domain.Models;
using Mermec.AnomalyDetector.Domain.Services;
using System.Diagnostics;

namespace Mermec.AnomalyDetector.Test
{
    public class AnomalyFinderServiceTests
    {
        [Fact]
        public void ThresholdAnomalyMeasurement_ShouldReturnEmptyArray()
        {
            //Arrange
            Measure[] repo = [];

            //Act
            IEnumerable<Anomaly> result = AnomalyFinderService.ThresholdAnomalyMeasurement(repo, 0);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ThresholdAnomalyMeasurement_ShouldThrowNullException()
        {
            //Arrange
            Measure[]? repo = null;

            //Act
            Action ThrowExceptionFunc = () => AnomalyFinderService.ThresholdAnomalyMeasurement(repo!, 0);

            //Assert
            Assert.Throws<NullReferenceException>(ThrowExceptionFunc);
        }

        [Fact]
        public void ThresholdAnomalyMeasurement_ShouldMatch()
        {
            //Arrange
            Measure[] input =
            [
                new Measure() { Index = 1, Latitude = new DMSPoint("N 45°29'12.0035\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9851\"", PointType.Longitude), DecayMeasure = 1.0000f },
                new Measure() { Index = 2, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9919\"", PointType.Longitude), DecayMeasure = 1.5000f },
                new Measure() { Index = 3, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9954\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 4, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0022\"", PointType.Longitude), DecayMeasure = 2.5000f},
                new Measure() { Index = 5, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0091\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 6, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0160\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 7, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0228\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 8, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0297\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 9, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0331\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 10, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0400\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 11, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0469\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 12, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0537\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 13, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0606\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 14, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0675\"", PointType.Longitude), DecayMeasure = 6.0000f},
                new Measure() { Index = 15, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0743\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 16, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0778\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 17, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0846\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 18, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0915\"", PointType.Longitude), DecayMeasure = 5.0000f},
                new Measure() { Index = 19, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0984\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 20, Latitude = new DMSPoint("N 45°29'12.1408\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1052\"", PointType.Longitude), DecayMeasure = 1.0000f},
            ];

            //Act
            IEnumerable<Anomaly> result = AnomalyFinderService.ThresholdAnomalyMeasurement(input, 2);

            //Assert
            Assert.Equal(3, result.Count());

            //Dalla misura 4 alla misura 7, con valore massimo 4.0000 alla posizione 6 e lunghezza anomalia 0.40 m;
            Anomaly firstResult = result.First();
            Assert.Equal(4, firstResult.FirstMeasure.Index);
            Assert.Equal(7, firstResult.LastMeasure.Index);
            Assert.Equal(6, firstResult.MaxMeasure.Index);
            Assert.Equal("4.0000", firstResult.MaxMeasure.DecayMeasure.ToString("N4"));
            Assert.Equal("0.40", firstResult.AnomalyLenght.ToString("N2"));

            //Dalla misura 13 alla misura 14, con valore massimo 6.0000 alla posizione 14 e lunghezza anomalia 0.20 m;
            Anomaly secondResult = result.ElementAt(1);
            Assert.Equal(13, secondResult.FirstMeasure.Index);
            Assert.Equal(14, secondResult.LastMeasure.Index);
            Assert.Equal(14, secondResult.MaxMeasure.Index);
            Assert.Equal("6.0000", secondResult.MaxMeasure.DecayMeasure.ToString("N4"));
            Assert.Equal("0.20", secondResult.AnomalyLenght.ToString("N2"));

            //Alla posizione 18, con valore massimo 5.000 alla posizione 18 e lunghezza anomalia 0.10 m.
            Anomaly thirdResult = result.ElementAt(2);
            Assert.Equal(18, thirdResult.FirstMeasure.Index);
            Assert.Equal(18, thirdResult.LastMeasure.Index);
            Assert.Equal("5.0000", thirdResult.MaxMeasure.DecayMeasure.ToString("N4"));
            Assert.Equal("0.10", thirdResult.AnomalyLenght.ToString("N2"));
        }

        [Fact]
        public void ClusterAnomalyMeasurement_ShouldMatch()
        {
            //Arrange
            Measure[] input =
            [
                new Measure() { Index = 1, Latitude = new DMSPoint("N 45°29'12.0035\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9851\"", PointType.Longitude), DecayMeasure = 1.0000f },
                new Measure() { Index = 2, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9919\"", PointType.Longitude), DecayMeasure = 1.5000f },
                new Measure() { Index = 3, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9954\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 4, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0022\"", PointType.Longitude), DecayMeasure = 2.5000f},
                new Measure() { Index = 5, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0091\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 6, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0160\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 7, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0228\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 8, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0297\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 9, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0331\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 10, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0400\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 11, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0469\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 12, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0537\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 13, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0606\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 14, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0675\"", PointType.Longitude), DecayMeasure = 6.0000f},
                new Measure() { Index = 15, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0743\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 16, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0778\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 17, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0846\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 18, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0915\"", PointType.Longitude), DecayMeasure = 5.0000f},
                new Measure() { Index = 19, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0984\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 20, Latitude = new DMSPoint("N 45°29'12.1408\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1052\"", PointType.Longitude), DecayMeasure = 1.0000f},
            ];

            //Act
            IEnumerable<Anomaly> result = AnomalyFinderService.ClusterAnomalyMeasurement(input, 2, 5);

            //Assert
            Assert.Equal(2, result.Count());
          
            Anomaly firstResult = result.First();
            Assert.Equal(4, firstResult.FirstMeasure.Index);
            Assert.Equal(7, firstResult.LastMeasure.Index);

            //L’anomalia aggregata partirà dall’indice 13 all’indice 18, sarà lunga 0.6 m e avrà valore massimo 6.000 all’indice 14.
            Anomaly secondResult = result.ElementAt(1);
            Assert.Equal(13, secondResult.FirstMeasure.Index);
            Assert.Equal(18, secondResult.LastMeasure.Index);
            Assert.Equal(14, secondResult.MaxMeasure.Index);
            Assert.Equal("6.0000", secondResult.MaxMeasure.DecayMeasure.ToString("N4"));
            Assert.Equal("0.60", secondResult.AnomalyLenght.ToString("N2"));
        }

        [Fact]
        public void ClusterAnomalyMeasurement_ShouldMatch_Extended()
        {
            //Arrange
            Measure[] input =
            [
                new Measure() { Index = 1, Latitude = new DMSPoint("N 45°29'12.0035\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9851\"", PointType.Longitude), DecayMeasure = 1.0000f },
                new Measure() { Index = 2, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9919\"", PointType.Longitude), DecayMeasure = 1.5000f },
                new Measure() { Index = 3, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9954\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 4, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0022\"", PointType.Longitude), DecayMeasure = 2.5000f},
                new Measure() { Index = 5, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0091\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 6, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0160\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 7, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0228\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 8, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0297\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 9, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0331\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 10, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0400\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 11, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0469\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 12, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0537\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 13, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0606\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 14, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0675\"", PointType.Longitude), DecayMeasure = 6.0000f},
                new Measure() { Index = 15, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0743\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 16, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0778\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 17, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0846\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 18, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0915\"", PointType.Longitude), DecayMeasure = 5.0000f},
                new Measure() { Index = 19, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0984\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 20, Latitude = new DMSPoint("N 45°29'12.1408\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1052\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 21, Latitude = new DMSPoint("N 45°29'12.1410\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1054\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 22, Latitude = new DMSPoint("N 45°29'12.1412\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1056\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 23, Latitude = new DMSPoint("N 45°29'12.1414\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1058\"", PointType.Longitude), DecayMeasure = 3.0000f},
            ];

            //Act
            IEnumerable<Anomaly> result = AnomalyFinderService.ClusterAnomalyMeasurement(input, 2, 5);

            //Assert
            Assert.Equal(2, result.Count());

            Anomaly firstResult = result.First();
            Assert.Equal(4, firstResult.FirstMeasure.Index);
            Assert.Equal(7, firstResult.LastMeasure.Index);

            //Si chiarisce che l’anomalia aggregata può essere ulteriormente aggregata ad altre anomalie successive purché la prossima misurazione sia all’indice 23 o inferiore (interna quindi alla distanza massima)
            Anomaly secondResult = result.ElementAt(1);
            Assert.Equal(13, secondResult.FirstMeasure.Index);
            Assert.Equal(23, secondResult.LastMeasure.Index);
            Assert.Equal(14, secondResult.MaxMeasure.Index);
            Assert.Equal("6.0000", secondResult.MaxMeasure.DecayMeasure.ToString("N4"));
        }

        [Fact]
        public void SafeDistanceAnomalyMeasurement_ShouldMatch()
        {
            //Arrange
            Measure[] input =
            [
                new Measure() { Index = 100, Latitude = new DMSPoint("N 45°29'12.6627\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.6408\"", PointType.Longitude), DecayMeasure = 1.0000f },
                new Measure() { Index = 101, Latitude = new DMSPoint("N 45°29'12.6627\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.6477\"", PointType.Longitude), DecayMeasure = 1.5000f },
                new Measure() { Index = 102, Latitude = new DMSPoint("N 45°29'12.6764\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.6545\"", PointType.Longitude), DecayMeasure = 1.0000f },
                new Measure() { Index = 103, Latitude = null, Longitude = null, DecayMeasure = 2.5000f},
                new Measure() { Index = 104, Latitude = null, Longitude = null, DecayMeasure = 3.0000f},
                new Measure() { Index = 105, Latitude = new DMSPoint("N 45°29'12.6901\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.6751\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 106, Latitude = new DMSPoint("N 45°29'12.7039\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.6820\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 107, Latitude = new DMSPoint("N 45°29'12.7039\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.6889\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 108, Latitude = new DMSPoint("N 45°29'12.7176\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.6957\"", PointType.Longitude), DecayMeasure = 1.0000f}
            ];

            //Act
            IEnumerable<Anomaly> result = AnomalyFinderService.SafeDistanceAnomalyMeasurement(input, 2, 5);

            //Assert
            Assert.Single(result);

            Anomaly singleResult = result.First();

            Measure firstMeasureComparison = input.First(i => i.Index == 102);
            Assert.Equal(firstMeasureComparison.Latitude, singleResult.FirstMeasure.Latitude);
            Assert.Equal(firstMeasureComparison.Longitude, singleResult.FirstMeasure.Longitude);

            Measure lastMeasureComparison = input.First(i => i.Index == 106);
            Assert.Equal(lastMeasureComparison.Latitude, singleResult.LastMeasure.Latitude);
            Assert.Equal(lastMeasureComparison.Longitude, singleResult.LastMeasure.Longitude);

            Assert.Equal(103, singleResult.FirstMeasure.Index);
        }

        [Fact]
        public void SafeDistanceAnomalyMeasurement_ShouldMatch2()
        {
            //Arrange
            Measure[] input =
            [
                new Measure() { Index = 200, Latitude = new DMSPoint("N 45°29'13.3218\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'20.3034\"", PointType.Longitude), DecayMeasure = 1.0000f },
                new Measure() { Index = 201, Latitude = new DMSPoint("N 45°29'13.3218\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'20.3103\"", PointType.Longitude), DecayMeasure = 1.5000f },
                new Measure() { Index = 202, Latitude = new DMSPoint("N 45°29'13.3356\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'20.3137\"", PointType.Longitude), DecayMeasure = 2.0000f },
                new Measure() { Index = 203, Latitude = new DMSPoint("N 45°29'13.3356\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'20.3206\"", PointType.Longitude), DecayMeasure = 2.5000f},
                new Measure() { Index = 204, Latitude = null, Longitude = null, DecayMeasure = 3.0000f},
                new Measure() { Index = 205, Latitude = null, Longitude = null, DecayMeasure = 2.5000f},
                new Measure() { Index = 206, Latitude = null, Longitude = null, DecayMeasure = 2.0000f},
                new Measure() { Index = 207, Latitude = new DMSPoint("N 45°29'13.3630\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'20.3481\"", PointType.Longitude), DecayMeasure = 1.5000f},
                new Measure() { Index = 208, Latitude = new DMSPoint("N 45°29'13.3768\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'20.3549\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 209, Latitude = new DMSPoint("N 45°29'13.3768\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'20.3618\"", PointType.Longitude), DecayMeasure = 0.5000f}
            ];

            //Act
            IEnumerable<Anomaly> result = AnomalyFinderService.SafeDistanceAnomalyMeasurement(input, 2, 5);

            //Assert
            Assert.Single(result);

            Anomaly singleResult = result.First();

            Measure firstMeasureComparison = input.First(i => i.Index == 203);
            Assert.Equal(firstMeasureComparison.Latitude, singleResult.FirstMeasure.Latitude);
            Assert.Equal(firstMeasureComparison.Longitude, singleResult.FirstMeasure.Longitude);

            Measure lastMeasureComparison = input.First(i => i.Index == 207);
            Assert.Equal(lastMeasureComparison.Latitude, singleResult.LastMeasure.Latitude);
            Assert.Equal(lastMeasureComparison.Longitude, singleResult.LastMeasure.Longitude);

            Assert.Equal(205, singleResult.LastMeasure.Index);
        }

        [Fact]
        public void ParallelClusterAnomalyMeasurement_ShouldMatch()
        {
            //Arrange
            Measure[] input =
            [
                new Measure() { Index = 1, Latitude = new DMSPoint("N 45°29'12.0035\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9851\"", PointType.Longitude), DecayMeasure = 1.0000f },
                new Measure() { Index = 2, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9919\"", PointType.Longitude), DecayMeasure = 1.5000f },
                new Measure() { Index = 3, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9954\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 4, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0022\"", PointType.Longitude), DecayMeasure = 2.5000f},
                new Measure() { Index = 5, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0091\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 6, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0160\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 7, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0228\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 8, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0297\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 9, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0331\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 10, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0400\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 11, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0469\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 12, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0537\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 13, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0606\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 14, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0675\"", PointType.Longitude), DecayMeasure = 6.0000f},
                new Measure() { Index = 15, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0743\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 16, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0778\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 17, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0846\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 18, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0915\"", PointType.Longitude), DecayMeasure = 5.0000f},
                new Measure() { Index = 19, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0984\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 20, Latitude = new DMSPoint("N 45°29'12.1408\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1052\"", PointType.Longitude), DecayMeasure = 1.0000f},
            ];

            //Act
            IEnumerable<Anomaly> result = AnomalyFinderService.ParallelClusterAnomalyMeasurement(input, 2, 5).OrderBy(i => i.FirstMeasure.Index);

            //Assert
            Assert.Equal(2, result.Count());

            Anomaly firstResult = result.First();
            Assert.Equal(4, firstResult.FirstMeasure.Index);
            Assert.Equal(7, firstResult.LastMeasure.Index);

            //L’anomalia aggregata partirà dall’indice 13 all’indice 18, sarà lunga 0.6 m e avrà valore massimo 6.000 all’indice 14.
            Anomaly secondResult = result.ElementAt(1);
            Assert.Equal(13, secondResult.FirstMeasure.Index);
            Assert.Equal(18, secondResult.LastMeasure.Index);
            Assert.Equal(14, secondResult.MaxMeasure.Index);
            Assert.Equal("6.0000", secondResult.MaxMeasure.DecayMeasure.ToString("N4"));
            Assert.Equal("0.60", secondResult.AnomalyLenght.ToString("N2"));
        }

        [Fact]
        public void ParallelClusterAnomalyMeasurement_ShouldMatch_Extended()
        {
            //Arrange
            Measure[] input =
            [
                new Measure() { Index = 1, Latitude = new DMSPoint("N 45°29'12.0035\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9851\"", PointType.Longitude), DecayMeasure = 1.0000f },
                new Measure() { Index = 2, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9919\"", PointType.Longitude), DecayMeasure = 1.5000f },
                new Measure() { Index = 3, Latitude = new DMSPoint("N 45°29'12.0172\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'18.9954\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 4, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0022\"", PointType.Longitude), DecayMeasure = 2.5000f},
                new Measure() { Index = 5, Latitude = new DMSPoint("N 45°29'12.0309\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0091\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 6, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0160\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 7, Latitude = new DMSPoint("N 45°29'12.0447\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0228\"", PointType.Longitude), DecayMeasure = 3.0000f},
                new Measure() { Index = 8, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0297\"", PointType.Longitude), DecayMeasure = 2.0000f},
                new Measure() { Index = 9, Latitude = new DMSPoint("N 45°29'12.0584\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0331\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 10, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0400\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 11, Latitude = new DMSPoint("N 45°29'12.0721\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0469\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 12, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0537\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 13, Latitude = new DMSPoint("N 45°29'12.0859\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0606\"", PointType.Longitude), DecayMeasure = 4.0000f},
                new Measure() { Index = 14, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0675\"", PointType.Longitude), DecayMeasure = 6.0000f},
                new Measure() { Index = 15, Latitude = new DMSPoint("N 45°29'12.0996\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0743\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 16, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0778\"", PointType.Longitude), DecayMeasure = 0.0000f},
                new Measure() { Index = 17, Latitude = new DMSPoint("N 45°29'12.1133\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0846\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 18, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0915\"", PointType.Longitude), DecayMeasure = 5.0000f},
                new Measure() { Index = 19, Latitude = new DMSPoint("N 45°29'12.1271\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.0984\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 20, Latitude = new DMSPoint("N 45°29'12.1408\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1052\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 21, Latitude = new DMSPoint("N 45°29'12.1410\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1054\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 22, Latitude = new DMSPoint("N 45°29'12.1412\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1056\"", PointType.Longitude), DecayMeasure = 1.0000f},
                new Measure() { Index = 23, Latitude = new DMSPoint("N 45°29'12.1414\"", PointType.Latitude), Longitude = new DMSPoint("E 9°12'19.1058\"", PointType.Longitude), DecayMeasure = 3.0000f},
            ];

            //Act
            IEnumerable<Anomaly> result = AnomalyFinderService.ParallelClusterAnomalyMeasurement(input, 2, 5).OrderBy(i => i.FirstMeasure.Index);

            //Assert
            Assert.Equal(2, result.Count());

            Anomaly firstResult = result.First();
            Assert.Equal(4, firstResult.FirstMeasure.Index);
            Assert.Equal(7, firstResult.LastMeasure.Index);

            //Si chiarisce che l’anomalia aggregata può essere ulteriormente aggregata ad altre anomalie successive purché la prossima misurazione sia all’indice 23 o inferiore (interna quindi alla distanza massima)
            Anomaly secondResult = result.ElementAt(1);
            Assert.Equal(13, secondResult.FirstMeasure.Index);
            Assert.Equal(23, secondResult.LastMeasure.Index);
            Assert.Equal(14, secondResult.MaxMeasure.Index);
            Assert.Equal("6.0000", secondResult.MaxMeasure.DecayMeasure.ToString("N4"));
        }
    }
}