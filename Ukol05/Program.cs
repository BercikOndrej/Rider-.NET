using System.Diagnostics;
using System.Globalization;
using System.Net;

class Program {
    static Dictionary<string, string> downloadedDatasets = new Dictionary<string, string>();
    
    static Dictionary<string, string> urls = new Dictionary<string, string>();


    static void Main(string[] args) {
        // Získání odkazů na daná data 
        urls.Add("Flags", "https://archive.ics.uci.edu/ml/machine-learning-databases/flags/flag.data");
        urls.Add("Brest cancer", "https://archive.ics.uci.edu/ml/machine-learning-databases/breast-cancer-wisconsin/breast-cancer-wisconsin.data");
        urls.Add("Mushroom", "https://archive.ics.uci.edu/ml/machine-learning-databases/mushroom/agaricus-lepiota.data");
        urls.Add("Zoo", "https://archive.ics.uci.edu/ml/machine-learning-databases/zoo/zoo.data");
        urls.Add("House votes", "https://archive.ics.uci.edu/ml/machine-learning-databases/voting-records/house-votes-84.data");
        
        // Sekvenční zpracování dat
        double seqTime = Math.Round(processAllDataSeq() / 1000, 4);
        
        // Zahození dat aby bylo porovnání zpracování dat férové
        downloadedDatasets.Clear();
        
        // Paralelní zpracování dat
        double paralelTime = Math.Round(processAllDataParalel() / 1000, 4);

        Console.WriteLine($"Sekvenční zpracování všech dat trvalo {seqTime} sekund");
        Console.WriteLine($"Paralelní zpracování všech dat trvalo {paralelTime} sekund");
    }
    
    // Metoda pro sekvenční zpracování dat
    static double processAllDataSeq() {
        Console.WriteLine("Sekvenční zpracování dat:\n");
        Stopwatch sw = Stopwatch.StartNew();
        
        DownloadDatasets(false);
        foreach (var item in downloadedDatasets) {
            processDataset(item.Key, item.Value);
        }
        
        return sw.ElapsedMilliseconds;
    }
    
    // Metoda pro paralelní zpracování dat
    static double processAllDataParalel() {
        Console.WriteLine("Paralelní zpracování dat:\n");
        Stopwatch sw = Stopwatch.StartNew();
        
        DownloadDatasets(true);
        ParallelOptions options = new ParallelOptions();
        options.MaxDegreeOfParallelism = 4;
        Parallel.ForEach(downloadedDatasets, options, item => {
            processDataset(item.Key, item.Value);
        });

        sw.Stop();
        return sw.ElapsedMilliseconds;
    }
    
    // Stáhnutí datasetů
    static void DownloadDatasets(bool useParallelism) {
        Console.WriteLine("Probíhá stahování datasetů...");

        if (useParallelism) {
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 4;
            Parallel.ForEach(urls, options, item => {
                try {
                    StreamReader reader = new StreamReader(WebRequest
                        .Create(item.Value)
                        .GetResponse()
                        .GetResponseStream()
                    );

                    string dataString = reader.ReadToEnd();
                    downloadedDatasets.Add(item.Key, dataString);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }
        else {
            foreach (var item in urls) {
                try {
                    StreamReader reader = new StreamReader(WebRequest
                        .Create(item.Value)
                        .GetResponse()
                        .GetResponseStream()
                    );

                    string dataString = reader.ReadToEnd();
                    downloadedDatasets.Add(item.Key, dataString);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        
        Console.WriteLine("Stahování bylo dokončeno.\n");
    }

    // Metoda pro zpracování jednoho datasetu -> jednotlivé informace vypíše do konzole
    static void processDataset(string datasetName, string dataString) {
        
        int fileSize = GetFileSize(urls.GetValueOrDefault(datasetName));
        
        int columnsCount = GetColumnsCount(dataString); 
        
        int rowsCount = GetRowsCount(dataString); 
        
        // Vypsání všech informací
        // Název datasetu
        Console.WriteLine($"Název datasetu: {datasetName}");
        
        // Velikost datasetu
        Console.WriteLine($"Velikost souboru: {fileSize}kB");
        
        // Počet sloupců
        Console.WriteLine($"Počet sloupců: {columnsCount}");

        // Počet řádků        
        Console.WriteLine($"Počet řádků: {rowsCount}");

        // Počet znaků + počet jejich výskytů
        writeCharactersInfo(dataString);
    }
    
    
    // Metoda pro zisk velikosti souboru
    static int GetFileSize(string url) {
        string sizeString = WebRequest.Create(url).GetResponse().Headers.Get("Content-Length");
        return (int) Math.Round(Double.Parse(sizeString) / 1024, 0);
    }
    

    // Metoda pro zisk počtu sloupců
    static int GetColumnsCount(string data) {
        string[] lines = data.Split("\n");
        return lines[1].Split(",").Length;
    }
    
    // Metoda pro zisk počtu řádků
    static int GetRowsCount(string data) {
        int rows = 0;
        string[] lines = data.Split("\n");
        foreach (var line in lines) {
            if (line != string.Empty) {
                rows++;
            }
        }
        
        return rows;
    }
    
    // Metoda která vypíše všechny znaky + jejich počet + celkový počet znaků
    static void writeCharactersInfo(string data) {
        int symbolsCount = 0;
        Dictionary<char, int> dict = new Dictionary<char, int>();
        char[] charArray = data.ToCharArray();
        
        foreach (var symbol in charArray) {
            if (dict.ContainsKey(symbol)) {
                int oldValue = dict.GetValueOrDefault(symbol);
                dict.Remove(symbol);
                dict.Add(symbol, oldValue + 1);
            }
            else {
                dict.Add(symbol, 1);
            }
            symbolsCount++;
        }
           
        // Vypsání dat
        foreach (var item in dict) {
            if (item.Key == '\n') {
                Console.WriteLine($"Znak \\n byl použit {item.Value}x");
            }
            else {
                Console.WriteLine($"Znak {item.Key} byl použit {item.Value}x");
            }
        }
        Console.WriteLine($"Celkový počet znaků: {symbolsCount} \n");
    }
}