using Mermec.AnomalyDetector.Application;
using Mermec.AnomalyDetector.Domain.Interfaces;
using Mermec.AnomalyDetector.Infrastructure.CSV;

namespace Mermec.AnomalyDetector.Console
{
    public enum ExerciseNumber
    {
        PrimoEsercizio = 1,
        SecondoEsercizio = 2,
        QuartoEsercizio = 4,
        QuintoEsercizio = 5,
    }

    public class BasicInteraction
    {
        public async Task Run()
        {
            string? filePath = null;
            float? thresholdValue = null;
            
            do
            {
                System.Console.WriteLine("================================================");

                if (string.IsNullOrEmpty(filePath))
                {
                    //Prendo il riferimento al file CSV delle misurazioni
                    filePath = GetFilePath();
                }

                if (thresholdValue is null)
                {
                    //Prendo il riferimento per la soglia limite di una anomalia
                    thresholdValue = GetThresholdValue();
                }

                //Chiedo quale esercizio si vuole eseguire
                ExerciseNumber exercise = GetExercise();

                await ProcessReport(filePath, thresholdValue.Value, exercise);

                System.Console.WriteLine("Press any button to repeat or x for exit");
            }
            while (System.Console.ReadLine() != "x");
        }

        private async Task ProcessReport(string filePath, float thresholdValue, ExerciseNumber exerciseNumber)
        {
            IMeasureRepository measureReport = new MeasureReportCVSAdapter(filePath);
            IAnomalyRepository anomalyReport = new AnomalyReportCSVAdapter(filePath);
            ExerciseHandler handler = new(measureReport, anomalyReport);

            string filename = string.Empty;

            switch (exerciseNumber)
            {
                case ExerciseNumber.PrimoEsercizio:
                    {
                        filename = await handler.Exercise1(thresholdValue);
                    }
                    break;
                case ExerciseNumber.SecondoEsercizio:
                    {
                        int clusterFactor = GetClusterFactor();
                        filename = await handler.Exercise2(thresholdValue, clusterFactor);
                    }
                    break;
                case ExerciseNumber.QuintoEsercizio:
                    {
                        int clusterFactor = GetClusterFactor();
                        filename = await handler.Exercise5(thresholdValue, clusterFactor);
                    }
                    break;
                case ExerciseNumber.QuartoEsercizio:
                    {
                        int clusterFactor = GetClusterFactor();
                        filename = await handler.Exercise4(thresholdValue, clusterFactor);
                    }
                    break;
                default:
                    break;
            }

            System.Console.WriteLine("CSV file created with filename {0}", filename);
        }

        private string GetFilePath()
        {
            System.Console.WriteLine("Enter the file path of the csv file");
            string filePath = System.Console.ReadLine() ?? string.Empty;

            if (!Path.Exists(filePath) || Path.GetExtension(filePath) != ".csv")
            {
                System.Console.WriteLine("File path is not correct. Please enter the correct file path");
                return GetFilePath();
            }

            return filePath;
        }

        private float GetThresholdValue()
        {
            System.Console.WriteLine("Enter the threshold value");

            string thresholdStringValue = System.Console.ReadLine() ?? string.Empty;

            if (!float.TryParse(thresholdStringValue, out float thresholdValue))
            {
                System.Console.WriteLine("Value is not correct. Please enter a correct number");
                return GetThresholdValue();
            }

            return thresholdValue;
        }

        private int GetClusterFactor()
        {
            System.Console.WriteLine("Enter the cluster factor");

            string clusterStringValue = System.Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(clusterStringValue, out int clusterValue))
            {
                System.Console.WriteLine("Value is not correct. Please enter a correct number");
                return GetClusterFactor();
            }

            return clusterValue;
        }

        private ExerciseNumber GetExercise()
        {
            System.Console.WriteLine("Enter the number exercise:");
            System.Console.WriteLine("Insert 1 for Threshold Anomaly Measurement (exercise 1)");
            System.Console.WriteLine("Insert 2 for Cluster Anomaly Measurement (exercise 2)");
            System.Console.WriteLine("Insert 4 for Safe Distance Anomaly Measurement (exercise 4)");
            System.Console.WriteLine("Insert 5 for Parallel Cluster Anomaly Measurement (exercise 5)");

            string? exercise = System.Console.ReadLine();
            if (!int.TryParse(exercise, out int number) || !Enum.IsDefined(typeof(ExerciseNumber), number))
            {
                System.Console.WriteLine("Value is not correct. Please enter a correct number");
                return GetExercise();
            }

            return (ExerciseNumber)number;
        }
    }        
}
