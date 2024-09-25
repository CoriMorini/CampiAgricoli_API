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

    public class NPKCampoMediaMese
    {
        public double? ValoreMedioN { get; set; }
        public double? ValoreMedioP { get; set; }
        public double? ValoreMedioK { get; set; }

        public double? deltaN { get; set; }
        public double? deltaP { get; set; }
        public double? deltaK { get; set; }
    }
    public class InfoCampoData
    {
        public double? N { get; set; }
        public double? P { get; set; }
        public double? K { get; set; }

        public double? Umidita { get; set; }

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


    public static int CalcolaPunteggioSalute(double NPK, double tempAmbiente, double tempSuolo, double umidita)
    {
        // 1. Valutazione NPK (range ideale: 50-70)
        double NPKScore = Math.Min(Math.Max((NPK - 50) / (70 - 50), 0), 1);

        // 2. Temperatura ambiente (range ideale: 20-30°C)
        double tempAmbienteScore = Math.Min(Math.Max((tempAmbiente - 20) / (30 - 20), 0), 1);

        // 3. Temperatura del suolo (range ideale: 30-35°C)
        double tempSuoloScore = Math.Min(Math.Max((tempSuolo - 30) / (35 - 30), 0), 1);

        // 4. Umidità (range ideale: 50-70%)
        double umiditaScore = Math.Min(Math.Max((umidita - 50) / (70 - 50), 0), 1);

        // Generazione di pesi casuali
        Random random = new Random();
        double pesoNPK = random.NextDouble();        // Peso casuale per NPK
        double pesoTempAmbiente = random.NextDouble(); // Peso casuale per tempAmbiente
        double pesoTempSuolo = random.NextDouble();   // Peso casuale per tempSuolo
        double pesoUmidita = random.NextDouble();     // Peso casuale per umidità

        // Normalizzazione dei pesi per far sì che la loro somma sia pari a 1
        double sommaPesi = pesoNPK + pesoTempAmbiente + pesoTempSuolo + pesoUmidita;
        pesoNPK /= sommaPesi;
        pesoTempAmbiente /= sommaPesi;
        pesoTempSuolo /= sommaPesi;
        pesoUmidita /= sommaPesi;

        // Calcolo del punteggio finale ponderato con pesi random
        double punteggioSalute =
            pesoNPK * NPKScore +
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
            List<VistaMisurazioniCampi> misurazioniCampi = db.VistaMisurazioniCampi.Where(x => x.IdUtente == idUtente).ToList();

            List<TabCampi> campiUtente = db.TabCampi.Where(x => x.IdUtente == idUtente).ToList();

            List<InfoCardCampo> infoCardCampi = new List<InfoCardCampo>();

            foreach (TabCampi campo in campiUtente)
            {
                InfoCardCampo tmp = new InfoCardCampo();

                tmp.nomeCampo = campo.NomeCampo;
                tmp.numeroMisurazioni = misurazioniCampi.Where(x => x.IdCampo == campo.IdCampo).Count();
                tmp.numeroErrori = new Random().Next(1, 9999);
                tmp.numeroMicrocontrolloriAttivi = db.TabMicrocontrollori.Where(x => x.IdCampo == campo.IdCampo).Count();

                /*
                tmp.saluteCampo = CalcolaPunteggioSalute(
                    db.VistaMisurazioniCampi.Where(x => x.IdCampo == campo.IdCampo && (x.IdTipologiaSensore == 1 || x.IdTipologiaSensore == 2 || x.IdTipologiaSensore == 3)).Average(x => x.valoreMisurazione) ?? 0,
                    db.VistaMisurazioniCampi.Where(x => x.IdCampo == campo.IdCampo && x.IdTipologiaSensore == 4).Average(x => x.valoreMisurazione) ?? 0,
                    db.VistaMisurazioniCampi.Where(x => x.IdCampo == campo.IdCampo && x.IdTipologiaSensore == 5).Average(x => x.valoreMisurazione) ?? 0,
                    db.VistaMisurazioniCampi.Where(x => x.IdCampo == campo.IdCampo && x.IdTipologiaSensore == 6).Average(x => x.valoreMisurazione) ?? 0
                );
                */

                tmp.saluteCampo = CalcolaPunteggioSalute(
                    new Random().Next(50, 70),
                    new Random().Next(20, 30),
                    new Random().Next(30, 35),
                    new Random().Next(50, 70)

                );


                // Aggiungere qui eventuali altre informazioni da visualizzare
                infoCardCampi.Add(tmp);
            }

            return infoCardCampi;

        };

    }


    [HttpGet("GetNPKcampoMediaMeseCorrente")]
    public ActionResult<NPKCampoMediaMese> GetNPKcampoMediaMeseCorrente(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi.Where(x => x.IdCampo == idCampo).ToList();


            double? mediaN = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Month == DateTime.Now.Month && x.IdTipologiaSensore == 1).Average(x => x.valoreMisurazione);
            double? mediaP = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Month == DateTime.Now.Month && x.IdTipologiaSensore == 2).Average(x => x.valoreMisurazione);
            double? mediaK = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Month == DateTime.Now.Month && x.IdTipologiaSensore == 3).Average(x => x.valoreMisurazione);

            double? mediaNPrecedente = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Month == DateTime.Now.Month - 1 && x.IdTipologiaSensore == 1).Average(x => x.valoreMisurazione);
            double? mediaPPrecedente = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Month == DateTime.Now.Month - 1 && x.IdTipologiaSensore == 2).Average(x => x.valoreMisurazione);
            double? mediaKPrecedente = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Month == DateTime.Now.Month - 1 && x.IdTipologiaSensore == 3).Average(x => x.valoreMisurazione);

            double? deltaN = (mediaN - mediaNPrecedente) / mediaNPrecedente;
            double? deltaP = (mediaP - mediaPPrecedente) / mediaPPrecedente;
            double? deltaK = (mediaK - mediaKPrecedente) / mediaKPrecedente;

            NPKCampoMediaMese risultato = new NPKCampoMediaMese();

            risultato.ValoreMedioN = mediaN.HasValue ? mediaN : 0;
            risultato.ValoreMedioP = mediaP.HasValue ? mediaP : 0;
            risultato.ValoreMedioK = mediaK.HasValue ? mediaK : 0;

            risultato.deltaN = deltaN.HasValue ? deltaN : 0;
            risultato.deltaP = deltaP.HasValue ? deltaP : 0;
            risultato.deltaK = deltaK.HasValue ? deltaK : 0;

            return Ok(risultato);

        };

    }

    [HttpGet("GetInfoCampoData")]
    public ActionResult<InfoCampoData> GetInfoCampoData(int idCampo, DateTime data)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi.Where(x => x.IdCampo == idCampo).ToList();

            double? N = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Day == data.Day && x.IdTipologiaSensore == 1).Average(x => x.valoreMisurazione);
            double? P = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Day == data.Day && x.IdTipologiaSensore == 2).Average(x => x.valoreMisurazione);
            double? K = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Day == data.Day && x.IdTipologiaSensore == 3).Average(x => x.valoreMisurazione);

            double? umidita = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Day == data.Day && x.IdTipologiaSensore == 4).Average(x => x.valoreMisurazione);

            double? temperaturaAmb = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Day == data.Day && x.IdTipologiaSensore == 5).Average(x => x.valoreMisurazione);

            double? temperaturaSuolo = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Day == data.Day && x.IdTipologiaSensore == 6).Average(x => x.valoreMisurazione);

            InfoCampoData risultato = new InfoCampoData();

            risultato.N = N.HasValue ? N : 0;
            risultato.P = P.HasValue ? P : 0;
            risultato.K = K.HasValue ? K : 0;
            risultato.Umidita = umidita.HasValue ? umidita : 0;
            risultato.TemperaturaAmb = temperaturaAmb.HasValue ? temperaturaAmb : 0;
            risultato.TemperaturaSuolo = temperaturaSuolo.HasValue ? temperaturaSuolo : 0;

            return Ok(risultato);

        }
    }

    [HttpGet("GetTemperaturaMediaAnnoCampo")]
    public ActionResult<List<double?>> GetTemperaturaMediaAnnoCampo(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi.Where(x => x.IdCampo == idCampo).ToList();

            List<TemperaturaMediaMese> temperatureMedie = new List<TemperaturaMediaMese>();

            for (int i = 1; i <= 12; i++)
            {
                double? temperatura = misurazioniCampo.Where(x => x.dataOraCertaMisurazione.Month == i && x.IdTipologiaSensore == 5).Average(x => x.valoreMisurazione);
                
                string mese = new DateTime(2021, i, 1).ToString("MMM", new System.Globalization.CultureInfo("it-IT"));

                temperatureMedie.Add(new TemperaturaMediaMese() { mese = mese, temperatura = temperatura.HasValue ? temperatura.Value : 0 });

            }

            return Ok(temperatureMedie);
        }
    }


}
