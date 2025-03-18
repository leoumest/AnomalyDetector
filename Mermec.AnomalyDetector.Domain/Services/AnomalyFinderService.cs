using Mermec.AnomalyDetector.Domain.Models;
using System.Collections.Concurrent;

namespace Mermec.AnomalyDetector.Domain.Services
{
    public class AnomalyFinderService
    {
        /// <summary>
        /// Find anomalies in a list of measurement with given threshold value.
        /// </summary>
        /// <param name="measures"></param>
        /// <param name="thresholdValue"></param>
        /// <returns></returns>
        public static IEnumerable<Anomaly> ThresholdAnomalyMeasurement(Measure[] measures, float thresholdValue)
        {
            List<Anomaly> result = [];
            Anomaly? currentAnomaly = null;
            int count = 0;

            for (int i = 0; i < measures.Length; i++)
            {
                //controllo se è una anomalia
                if (measures[i].DecayMeasure > thresholdValue)
                {
                    //controllo se è la prima anomalia
                    if(currentAnomaly is null)
                    {
                        //creo oggetto come se fosse l'unico nel range
                        currentAnomaly = new()
                        {
                            FirstMeasure = measures[i],
                            LastMeasure = measures[i],
                            MaxMeasure = measures[i],
                            AnomalyLenght = 0.10,
                            GeographicalLenght = null
                        };
                    }
                    else
                    {
                        //Non è la prima anomalia, la segno come ultima.
                        currentAnomaly.LastMeasure = measures[i];
                    }

                    //controllo se è l'anomalia con la misurazione più alta
                    if (measures[i].DecayMeasure > currentAnomaly.MaxMeasure.DecayMeasure) 
                        currentAnomaly.MaxMeasure = measures[i];

                    //aumento di 1 il contatore
                    count++;
                }
                else if(currentAnomaly is not null) 
                {
                    //la misurazione non presenta anomalie ma ne ha trovato uno o + in precedenza

                    //se esistono più anomalie nel range calcolo le distanze
                    CalculateDistances(currentAnomaly, count);

                    //aggiungo alla lista delle anomalie
                    result.Add(currentAnomaly);

                    //resetto il puntatore e il contatore del range
                    currentAnomaly = null;
                    count = 0;
                }
            }

            return result;
        }

        /// <summary>
        /// Find anomalies in a list of measurement with given threshold value aggregated based on integer range factor.
        /// </summary>
        /// <param name="measures"></param>
        /// <param name="thresholdValue"></param>
        /// <param name="clusterFactor"></param>
        /// <returns></returns>
        public static IEnumerable<Anomaly> ClusterAnomalyMeasurement(Measure[] measures, float thresholdValue, int clusterFactor)
        {
            List<Anomaly> result = [];
            for (int i = 0; i < measures.Length; i++)
            {
                //cerco la prima anomalia
                bool IsAnomaly = measures[i].DecayMeasure > thresholdValue;
                if (IsAnomaly)
                {
                    //creo oggetto come se fosse l'unico nel range
                    Anomaly anomaly = new()
                    {
                        FirstMeasure = measures[i],
                        LastMeasure = measures[i],
                        MaxMeasure = measures[i],
                        AnomalyLenght = 0.10,
                        GeographicalLenght = null
                    };

                    int count = 1;

                    //Assegno l'indice massimo del range di aggregazione SE in quella posizione si trova una anomalia
                    int clusterMaxIndex = i;
                    if (HasAnomalyAtMaxRangeForward(i, clusterFactor, measures, thresholdValue))
                    {
                        clusterMaxIndex = i + clusterFactor;
                    }

                    //ciclo le prossime anomalie e/o le misurazioni che sono nel range del cluster
                    for (int j = i + 1; j < measures.Length && (measures[j].DecayMeasure > thresholdValue || (j < clusterMaxIndex && clusterMaxIndex < measures.Length)); j++)
                    {
                        bool _IsAnomaly = measures[j].DecayMeasure > thresholdValue;

                        //controllo se c'è un altra anomalia nel range massimo, se presente allungo il range fino a quella misura
                        if (_IsAnomaly && HasAnomalyAtMaxRangeForward(j, clusterFactor, measures, thresholdValue))
                        {
                            clusterMaxIndex = j + clusterFactor;
                        }

                        //aggiorno l'ultima anomalia
                        anomaly.LastMeasure = measures[j];

                        //controllo se l'anomalia di questa misurazione è superiore alle precedenti nel range di osservazione
                        if (anomaly.MaxMeasure.DecayMeasure < measures[j].DecayMeasure)
                            anomaly.MaxMeasure = measures[j];

                        count++;
                    }

                    //se esistono più anomalie nel range calcolo le distanze
                    CalculateDistances(anomaly, count);

                    //reimposto l'index del ciclo inziale escludendo le misure che ho già analizzato
                    i = i + count - 1;

                    //aggiungo alla lista delle anomalie
                    result.Add(anomaly);
                }
            }

            return result;
        }

        /// <summary>
        /// Find anomalies in a list of measurement with given threshold value aggregated based on integer range factor, using Parallel calculations.
        /// </summary>
        /// <param name="measures"></param>
        /// <param name="thresholdValue"></param>
        /// <param name="clusterFactor"></param>
        /// <returns></returns>
        public static IEnumerable<Anomaly> ParallelClusterAnomalyMeasurement(Measure[] measures, float thresholdValue, int clusterFactor)
        {
            ConcurrentDictionary<int,Anomaly> result = [];
            Parallel.For(0, measures.Length, i =>
            {
                bool IsAnomaly = measures[i].DecayMeasure > thresholdValue;
                if (IsAnomaly)
                {
                    //controllo che sia la prima anomalia nelle vicinanze
                    bool IsPrecedentMeasureAnomaly = i > 0 && measures[i - 1].DecayMeasure > thresholdValue;
                    if(IsPrecedentMeasureAnomaly || HasAnomalyAtMaxRangeBackward(i, clusterFactor, measures, thresholdValue))
                    {
                        i = GetFirstClusterIndex(i, clusterFactor, measures, thresholdValue);                
                    }

                    //creo oggetto come se fosse l'unico nel range
                    Anomaly anomaly = new()
                    {
                        FirstMeasure = measures[i],
                        LastMeasure = measures[i],
                        MaxMeasure = measures[i],
                        AnomalyLenght = 0.10,
                        GeographicalLenght = null
                    };

                    int count = 1;

                    //Assegno l'indice massimo del range di aggregazione SE in quella posizione si trova una anomalia
                    int clusterMaxIndex = i;
                    if (HasAnomalyAtMaxRangeForward(i, clusterFactor, measures, thresholdValue))
                    {
                        clusterMaxIndex = i + clusterFactor;
                    }

                    //ciclo le prossime anomalie e/o le misurazioni che sono nel range del cluster
                    for (int j = i + 1; j < measures.Length && (measures[j].DecayMeasure > thresholdValue || (j < clusterMaxIndex && clusterMaxIndex < measures.Length)); j++)
                    {
                        bool _IsAnomaly = measures[j].DecayMeasure > thresholdValue;

                        //controllo se c'è un altra anomalia nel range massimo, se presente allungo il range fino a quella misura
                        if (_IsAnomaly && HasAnomalyAtMaxRangeForward(j, clusterFactor, measures, thresholdValue))
                        {
                            clusterMaxIndex = j + clusterFactor;
                        }

                        //aggiorno l'ultima anomalia
                        anomaly.LastMeasure = measures[j];

                        //controllo se l'anomalia di questa misurazione è superiore alle precedenti nel range di osservazione
                        if (anomaly.MaxMeasure.DecayMeasure < measures[j].DecayMeasure)
                            anomaly.MaxMeasure = measures[j];

                        count++;
                    }

                    //se esistono più anomalie nel range calcolo le distanze
                    CalculateDistances(anomaly, count);

                    //aggiungo alla lista delle anomalie
                    result.AddOrUpdate(anomaly.FirstMeasure.Index, (i) => anomaly, (i, a) => a);
                }
            });
                      
            return result.Values;
        }

        /// <summary>
        /// Find anomalies in a list of measurement with given threshold value aggregated based on integer range factor.
        /// If both start and the end of the anomaly don't have geographic coordinates, it finds the closest one.
        /// </summary>
        /// <param name="measures"></param>
        /// <param name="thresholdValue"></param>
        /// <param name="clusterFactor"></param>
        /// <returns></returns>
        public static IEnumerable<Anomaly> SafeDistanceAnomalyMeasurement(Measure[] measures, float thresholdValue, int clusterFactor)
        {
            List<Anomaly> result = [];
            for (int i = 0; i < measures.Length; i++)
            {
                //cerco la prima anomalia
                bool IsAnomaly = measures[i].DecayMeasure > thresholdValue;
                if (IsAnomaly)
                {
                    //creo oggetto come se fosse l'unico nel range
                    Anomaly anomaly = new()
                    {
                        FirstMeasure = measures[i],
                        LastMeasure = measures[i],
                        MaxMeasure = measures[i],
                        AnomalyLenght = 0.10,
                        GeographicalLenght = null
                    };

                    int count = 1;

                    //Assegno l'indice massimo del range di aggregazione SE in quella posizione si trova una anomalia
                    int clusterMaxIndex = i;
                    if (HasAnomalyAtMaxRangeForward(i, clusterFactor, measures, thresholdValue))
                    {
                        clusterMaxIndex = i + clusterFactor;
                    }

                    //ciclo le misurazioni seguenti e/o le misurazioni che sono nel range del cluster
                    for (int j = i + 1; j < measures.Length && (measures[j].DecayMeasure > thresholdValue || (j < clusterMaxIndex && clusterMaxIndex < measures.Length)); j++)
                    {
                        bool _IsAnomaly = measures[j].DecayMeasure > thresholdValue;

                        //controllo se c'è un altra anomalia nel range massimo, se presente allungo il range fino a quella misura
                        if (_IsAnomaly && HasAnomalyAtMaxRangeForward(j, clusterFactor, measures, thresholdValue))
                        {
                            clusterMaxIndex = j + clusterFactor;
                        }

                        //aggiorno l'ultima anomalia
                        anomaly.LastMeasure = measures[j];

                        //controllo se l'anomalia di questa misurazione è superiore alle precedenti nel range di osservazione
                        if (anomaly.MaxMeasure.DecayMeasure < measures[j].DecayMeasure)
                            anomaly.MaxMeasure = measures[j];

                        count++;
                    }

                    //se le coordinate iniziali sono mancanti cerco le coordinate iniziali più vicine
                    if(anomaly.FirstMeasure.Longitude is null || anomaly.FirstMeasure.Latitude is null)
                    {
                        Measure closestAnomaly = GetAnomalyWithCoordinatesClosestToStart(i, measures, clusterFactor);

                        if(closestAnomaly != anomaly.FirstMeasure)
                        {
                            //devo creare un nuovo oggetto altrimenti inquino le misure iniziali con possibili bug molto ostici da trovare
                            anomaly.FirstMeasure = new Measure
                            {
                                Index = anomaly.FirstMeasure.Index,
                                Latitude = closestAnomaly.Latitude,
                                Longitude = closestAnomaly.Longitude,
                                DecayMeasure = anomaly.FirstMeasure.DecayMeasure
                            };
                        }
                    }

                    //idem per quelle finali
                    if (anomaly.LastMeasure.Longitude is null || anomaly.LastMeasure.Latitude is null)
                    {
                        Measure closestAnomaly = GetAnomalyWithCoordinatesClosestToEnd(i + count, measures, clusterFactor);
                        if (closestAnomaly != anomaly.LastMeasure)
                        {
                            //devo creare un nuovo oggetto altrimenti inquino le misure iniziali con possibili bug molto ostici da trovare
                            anomaly.LastMeasure = new Measure
                            {
                                Index = anomaly.LastMeasure.Index,
                                Latitude = closestAnomaly.Latitude,
                                Longitude = closestAnomaly.Longitude,
                                DecayMeasure = anomaly.LastMeasure.DecayMeasure
                            };
                        }
                    }

                    //se esistono più anomalie nel range calcolo le distanze
                    CalculateDistances(anomaly, count);

                    //reimposto l'index del ciclo inziale escludendo le misure che ho già analizzato
                    i = i + count - 1;

                    //aggiungo alla lista delle anomalie
                    result.Add(anomaly);
                }
            }

            return result;
        }

        private static int GetFirstClusterIndex(int i, int clusterFactor, Measure[] measures, float thresholdValue)
        {
            bool IsPrecedentMeasureAnomaly = i > 0 && measures[i - 1].DecayMeasure > thresholdValue;
            if (IsPrecedentMeasureAnomaly)
            {
                return GetFirstClusterIndex(i - 1, clusterFactor, measures, thresholdValue);
            }
            if (HasAnomalyAtMaxRangeBackward(i, clusterFactor, measures, thresholdValue))
            {
                return GetFirstClusterIndex(i - clusterFactor, clusterFactor, measures, thresholdValue);
            }

            return i;
        }

        private static void CalculateDistances(Anomaly anomaly, int count)
        {
            if (anomaly.FirstMeasure != anomaly.LastMeasure)
            {
                //conto la distanza tra il punto iniziale e il punto finale
                anomaly.AnomalyLenght = anomaly.AnomalyLenght * count;

                //conto la distanza in linea d'aria tra il punto iniziale e il punto finale (se sono presenti entrambe le misurazioni geografiche)
                if (anomaly.FirstMeasure.TryGetDoubleCoordinates(out double originLatitude, out double originLongitude) &&
                    anomaly.LastMeasure.TryGetDoubleCoordinates(out double destinationLatitude, out double destinationLongitude))
                {
                    anomaly.GeographicalLenght = CoordinateService.GetDistanceInMetersV3(originLongitude, originLatitude, destinationLongitude, destinationLatitude);
                }
            }
        }

        private static bool HasAnomalyAtMaxRangeForward(int currentIndex, int clusterFactor, Measure[] measures, float thresholdValue)
        {
            int range = currentIndex + clusterFactor;
            return range < measures.Length && measures[range].DecayMeasure > thresholdValue;
        }

        private static bool HasAnomalyAtMaxRangeBackward(int currentIndex, int clusterFactor, Measure[] measures, float thresholdValue)
        {
            int range = currentIndex - clusterFactor;
            return range >= 0 && measures[range].DecayMeasure > thresholdValue;
        }

        private static Measure GetAnomalyWithCoordinatesClosestToStart(int currentIndex, Measure[] measures, int clusterFactor)
        {
            Measure measure = measures[currentIndex];
            if (measure.Latitude is null && measure.Longitude is null)
            {
                for (int i = 0; i < measures.Length && i < clusterFactor; i++)
                {
                    if (measures[currentIndex + i].Latitude is not null && measures[currentIndex + i].Longitude is not null)
                        return measures[currentIndex + i];

                    if (measures[currentIndex - i].Latitude is not null && measures[currentIndex - i].Longitude is not null)
                        return measures[currentIndex - i];
                }
            }

            return measure;
        }

        private static Measure GetAnomalyWithCoordinatesClosestToEnd(int currentIndex, Measure[] measures, int clusterFactor)
        {
            Measure measure = measures[currentIndex];
            if (measure.Latitude is null && measure.Longitude is null)
            {
                for (int i = 0; i < measures.Length && i < clusterFactor; i++)
                {
                    if (measures[currentIndex - i].Latitude is not null && measures[currentIndex - i].Longitude is not null)
                        return measures[currentIndex - i];

                    if (measures[currentIndex + i].Latitude is not null && measures[currentIndex + i].Longitude is not null)
                        return measures[currentIndex + i];
                }
            }

            return measure;
        }
    }
}