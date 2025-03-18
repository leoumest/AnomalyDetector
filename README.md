# Mermec.AnomalyDetector.Console
Console application che dato in input un CSV e un valore di numerico di soglia (Threshold value) trova le anomalie in esse contenute.
Per eseguire la console è necessario eseguire l'exe generato dalla compilazione del progetto Console.
La shell vi chiederà all'inizio di inserire il path assoluto del file CSV che volete analizzare:

> ================================================
> 
> Enter the file path of the csv file

Una volta inserito il valore corretto vi chiederà di inserire il valore numerico intero del valore soglia:

> Enter the threshold value

A questo punto vi chiederà di inserire con quale modalità effettuare la ricerca:

> Enter the number exercise:
> 
> Insert 1 for Threshold Anomaly Measurement (exercise 1)
> 
> Insert 2 for Cluster Anomaly Measurement (exercise 2)
> 
> Insert 4 for Safe Distance Anomaly Measurement (exercise 4)
> 
> Insert 5 for Parallel Cluster Anomaly Measurement (exercise 5)

Qualora si voglia eseguire un esercizio con aggregazione delle misure (tutti gli esercizi tranne il 1), vi verrà chiesto di inserire un parametro numerico intero di aggregazione (Cluster factor):

> Enter the cluster factor

Se non genera un'esplosione nucleare vi restituirà una stringa mostrandovi il nome univoco del file che ha creato nella stessa cartella del file CSV di origine.

> CSV file created with filename Report20250318110058.csv
> 
> Press any button to repeat or x for exit

A questo punto è possibile premere x per terminare l'applicazione oppure, premendo qualsiasi altro pulsante, ripetere l'esecuzione selezionando un altro esercizio, mantenendo lo stesso *File Path* di origine e *Threshold value*.

## Code Coverage

![image](https://github.com/user-attachments/assets/5b664dd4-b218-4139-b737-09f68e02dece)
