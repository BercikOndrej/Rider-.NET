
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Newtonsoft.Json;

namespace Ukol01;

public class Program {

    static void Main() {
        // Stáhnutí souboru programově a získání příslušného streamu
        List<FlagData> flagList = new List<FlagData>();

        try {
            StreamReader reader = new StreamReader(WebRequest
                .Create("https://archive.ics.uci.edu/ml/machine-learning-databases/flags/flag.data")
                .GetResponse()
                .GetResponseStream()
            );

            // Zpracování dat
            string line = reader.ReadLine();

            while (line != null) {
                var dataArray = line.Split(",");

                FlagData data = new FlagData(
                    dataArray[0],
                    uint.Parse(dataArray[1]),
                    uint.Parse(dataArray[2]),
                    uint.Parse(dataArray[3]),
                    uint.Parse(dataArray[4]),
                    uint.Parse(dataArray[5]),
                    uint.Parse(dataArray[6]),
                    uint.Parse(dataArray[7]),
                    uint.Parse(dataArray[8]),
                    uint.Parse(dataArray[9]),
                    int.Parse(dataArray[10]) == 1,
                    int.Parse(dataArray[11]) == 1,
                    int.Parse(dataArray[12]) == 1,
                    int.Parse(dataArray[13]) == 1,
                    int.Parse(dataArray[14]) == 1,
                    int.Parse(dataArray[15]) == 1,
                    int.Parse(dataArray[16]) == 1,
                    dataArray[17],
                    uint.Parse(dataArray[18]),
                    uint.Parse(dataArray[19]),
                    uint.Parse(dataArray[20]),
                    uint.Parse(dataArray[21]),
                    uint.Parse(dataArray[22]),
                    int.Parse(dataArray[23]) == 1,
                    int.Parse(dataArray[24]) == 1,
                    int.Parse(dataArray[25]) == 1,
                    int.Parse(dataArray[26]) == 1,
                    int.Parse(dataArray[27]) == 1,
                    dataArray[28],
                    dataArray[29]
                );

                flagList.Add(data);
                line = reader.ReadLine();
            }

            reader.Close();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }

        // Anglicky mluvící země seřazeny sestupně podle počtu hvězd -> vypisuji pouze jména
        var englishCountries = flagList
            .Where(c => (c.Language == (uint)FlagData.ELanguages.ENGLISH && c.BluePresence && c.StarsOrSunsNumber != 0))
            .OrderByDescending(c => c.StarsOrSunsNumber);
        Console.WriteLine("Země, které mluví anglicky, mají ve vlajce modrou barvu a také nějakou hvězdu:");
        foreach (var country in englishCountries) {
            Console.WriteLine(country.Name);
        }

        // JSON
        string jsonFileName = "result.json";
        string basePath = AppContext.BaseDirectory;
        string finalPath = Path.Combine(basePath, jsonFileName);

        var resultFlags = flagList
            .Select(f => new { f.Name, f.StarsOrSunsNumber, f.ColorsNumber, f.PopulationInMillions });

        string resultJson = JsonConvert.SerializeObject(resultFlags);

        // Vytvoření a zapsání dat do souboru json
        using (var jsonWriter = new StreamWriter(finalPath)) {
            jsonWriter.Write(resultJson);
            jsonWriter.Flush();
        }

        // Odeslání emailu
        try {
            MailMessage newMail = new MailMessage();

            // Přidání přílohy
            newMail.Attachments.Add(new Attachment(finalPath));
            
            // Využití smtp serveru pro outlook
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
            client.Port = 587;
            client.EnableSsl = true;
        
            // Nastavení emailu
            newMail.From = new MailAddress("bercon00@upol.cz", "Ondřej Berčík");
            newMail.To.Add("radek.janostik@upol.cz"); 
            newMail.Subject = "PNE -- výsledky -- Ondřej Berčík";
        
            // Autentizace 
            client.Credentials = new NetworkCredential("bercon00@upol.cz", "myPassword");
        
            // Odeslání emailu
            client.Send(newMail); 
            Console.WriteLine("Email Sent");
        }
        
        catch (Exception ex)
        {
            Console.WriteLine("Error -" +ex);
        }
        
    }
}
