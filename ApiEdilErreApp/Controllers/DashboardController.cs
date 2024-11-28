using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using static ApiEdilErreApp.Controllers.ReportController;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class DashboardController : ControllerBase
{

    #region data models e funzioni di appoggio

    
    public class InfoCampoData
    {
  
        public double? UmiditaAmb { get; set; }

        public double? UmiditaTer { get; set; }

        public double? TemperaturaAmb { get; set; }

        public double? TemperaturaSuolo { get; set; }

    }
    public class InfoCardCampo 
    {
        public string nomeCampo { get; set; }
        public int saluteCampo { get; set; }
        public int numeroMisurazioni { get; set; }
        public int numeroErrori { get; set; }
        public int numeroMicrocontrolloriAttivi { get; set; }
    }
    public class TemperaturaMediaMese 
    {
        public string mese { get; set; }
        public double temperatura { get; set; }
    }


    public static int CalcolaPunteggioSalute(double umiditaAmb, double umiditaTer, double tempAmb, double tempTer)
    {
        // 1. Temperatura ambiente (range ideale: 18-30°C)
        double tempAmbienteScore = Math.Min(Math.Max((tempAmb - 18) / (30 - 18), 0), 1);

        // 2. Temperatura del suolo (range ideale: 25-30°C, limite massimo: 35°C)
        double tempSuoloScore = Math.Min(Math.Max((tempTer - 25) / (30 - 25), 0), 1);

        // 3. Umidità ambiente (range ideale: 50-70%)
        double umiditaAmbScore = Math.Min(Math.Max((umiditaAmb - 50) / (70 - 50), 0), 1);

        // 4. Umidità del terreno (range ideale: 50-70%)
        double umiditaTerScore = Math.Min(Math.Max((umiditaTer - 50) / (70 - 50), 0), 1);

        // Calcolare la media dei punteggi di umidità
        double umiditaScore = (umiditaAmbScore + umiditaTerScore) / 2;

        // Generazione di pesi casuali
        Random random = new Random();
        double pesoTempAmbiente = random.NextDouble(); // Peso casuale per tempAmbiente
        double pesoTempSuolo = random.NextDouble();    // Peso casuale per tempSuolo
        double pesoUmidita = random.NextDouble();      // Peso casuale per umidità

        // Normalizzazione dei pesi per far sì che la loro somma sia pari a 1
        double sommaPesi = pesoTempAmbiente + pesoTempSuolo + pesoUmidita;
        pesoTempAmbiente /= sommaPesi;
        pesoTempSuolo /= sommaPesi;
        pesoUmidita /= sommaPesi;

        // Calcolo del punteggio finale ponderato con pesi random
        double punteggioSalute =
            pesoTempAmbiente * tempAmbienteScore +
            pesoTempSuolo * tempSuoloScore +
            pesoUmidita * umiditaScore;

        // Convertire il punteggio in un range da 0 a 100
        return (int)Math.Round(punteggioSalute * 100);
    }




    #endregion


    [HttpGet("GetListCardCampi")]
    public ActionResult<List<InfoCardCampo>> GetListCardCampi(int idUtente)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniUtente> misurazioniCampi = db.VistaMisurazioniUtente.Where(x => x.IdUtente == idUtente).ToList();

            List<TabCampi> campiUtente = db.TabCampi.Where(x => x.IdUtente == idUtente).ToList();

            List<InfoCardCampo> infoCardCampi = new List<InfoCardCampo>();

            foreach (TabCampi campo in campiUtente)
            {
                InfoCardCampo tmp = new InfoCardCampo();

                tmp.nomeCampo = campo.NomeCampo;
                tmp.numeroMisurazioni = misurazioniCampi.Where(x => x.IdCampo == campo.IdCampo).Count();
                tmp.numeroErrori = new Random().Next(1, 9999);
                tmp.numeroMicrocontrolloriAttivi = db.TabMicrocontrollori.Where(x => x.IdCampo == campo.IdCampo).Count();


                tmp.saluteCampo = CalcolaPunteggioSalute(
                    db.VistaMisurazioniUtente.Where(x => x.IdCampo == campo.IdCampo && x.IdTipologiaSensore == 1).Average(x => x.valoreMisurazione) ?? 0,
                    db.VistaMisurazioniUtente.Where(x => x.IdCampo == campo.IdCampo && x.IdTipologiaSensore == 2).Average(x => x.valoreMisurazione) ?? 0,
                    db.VistaMisurazioniUtente.Where(x => x.IdCampo == campo.IdCampo && x.IdTipologiaSensore == 3).Average(x => x.valoreMisurazione) ?? 0,
                    db.VistaMisurazioniUtente.Where(x => x.IdCampo == campo.IdCampo && x.IdTipologiaSensore == 4).Average(x => x.valoreMisurazione) ?? 0
                );


                // Aggiungere qui eventuali altre informazioni da visualizzare
                infoCardCampi.Add(tmp);
            }

            return infoCardCampi;

        };

    }



    [HttpGet("GetInfoCampoData")]
    public ActionResult<InfoCampoData> GetInfoCampoData(int idCampo, DateTime data)
    {
        using (var db = new CampiAgricoliContext())
        {
            // Recupero tutte le misurazioni del campo per il giorno specificato
            List<VistaMisurazioniUtente> misurazioniCampo = db.VistaMisurazioniUtente
                .Where(x => x.IdCampo == idCampo && x.dataOraCertaMisurazione.Date == data.Date)
                .ToList();

            // Calcolo della media per i vari sensori, se esistono dati
            double? umiditaAmb = misurazioniCampo
                .Where(x => x.IdTipologiaSensore == 4)  // Sensore umidità ambiente
                .Average(x => x.valoreMisurazione);

            double? temperaturaAmb = misurazioniCampo
                .Where(x => x.IdTipologiaSensore == 5)  // Sensore temperatura ambiente
                .Average(x => x.valoreMisurazione);

            double? temperaturaSuolo = misurazioniCampo
                .Where(x => x.IdTipologiaSensore == 6)  // Sensore temperatura suolo
                .Average(x => x.valoreMisurazione);

            // Creazione del risultato con i dati calcolati o 0 se i valori non sono disponibili
            InfoCampoData risultato = new InfoCampoData
            {
                UmiditaAmb = umiditaAmb ?? 0,   // Se non ci sono dati, assegno 0
                TemperaturaAmb = temperaturaAmb ?? 0,
                TemperaturaSuolo = temperaturaSuolo ?? 0
            };

            // Restituisco il risultato
            return Ok(risultato);
        }
    }

    [HttpGet("GetTemperaturaMediaAnnoCampo")]
    public ActionResult<List<double?>> GetTemperaturaMediaAnnoCampo(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniUtente> misurazioniCampo = db.VistaMisurazioniUtente.Where(x => x.IdCampo == idCampo).ToList();

            List<TemperaturaMediaMese> temperatureMedie = new List<TemperaturaMediaMese>();

            for (int i = 1; i <= 12; i++)
            {
                double? temperatura = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Month == i && x.IdTipologiaSensore == 3).Average(x => x.valoreMisurazione);
                
                string mese = new DateTime(2021, i, 1).ToString("MMM", new System.Globalization.CultureInfo("it-IT"));

                temperatureMedie.Add(new TemperaturaMediaMese() { mese = mese, temperatura = temperatura.HasValue ? temperatura.Value : 0 });

            }

            return Ok(temperatureMedie);
        }
    }


}
