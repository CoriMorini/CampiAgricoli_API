using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiEdilErreApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrendController : ControllerBase
    {
        #region data models e funzioni di appoggio

        // Modello per il punteggio dei vari nutrienti e condizioni
        public class DataInfoTrend
        {
            public int PunteggioSalute { get; set; }

            public List<double> MisurazioniAnnuali { get; set; }
        }

        // Funzione generica per calcolare il punteggio basato su valori
        private static int CalcolaPunteggioSalute(double valoreMedio, double valoreIdeale)
        {
            // Calcola la percentuale di valore medio rispetto al valore ideale
            double punteggio = (valoreMedio / valoreIdeale) * 100;

            // Assicurati che il punteggio non superi 100
            return (int)Math.Min(punteggio, 100);
        }

        // Valori ideali per i nutrienti e le condizioni
        private const double ValoreIdealeN = 200; // Valore ideale per il azoto
        private const double ValoreIdealeFosforo = 60; // Valore ideale per il fosforo
        private const double ValoreIdealePotassio = 300; // Valore ideale per il potassio
        private const double ValoreIdealeUmidita = 50; // Valore ideale per l'umidità (in %)
        private const double ValoreIdealeTempAmbiente = 25; // Valore ideale per la temperatura ambiente (in °C)
        private const double ValoreIdealeTempSuolo = 20; // Valore ideale per la temperatura del suolo (in °C)

        #endregion

        
        [HttpGet("GetTrendN")]
        public ActionResult<DataInfoTrend> GetTrendN(int idCampo)
        {
            using (var db = new CampiAgricoliContext())
            {
                try
                {

                    List<VistaMisurazioniCampi> listaMisurazioniCampi = db.VistaMisurazioniCampi.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 1).ToList();

                    double media = listaMisurazioniCampi.Average(x => x.valoreMisurazione) ?? 0;

                    int punteggio = CalcolaPunteggioSalute(media, ValoreIdealeN);

                    List<double> misurazioniAnnuali = new List<double>();

                    for (int i = 1; i <= 12; i++ )
                    {
                        misurazioniAnnuali[i] = listaMisurazioniCampi.Where(x => x.dataOraCertaMisurazione.Month == i).Average(x => x.valoreMisurazione) ?? 0;
                    }


                    return Ok(new DataInfoTrend
                    {
                        PunteggioSalute = punteggio,
                        MisurazioniAnnuali = misurazioniAnnuali

                    });

                }
                catch (Exception e)
                {
                    return NotFound(e.Message);
                }
            }
        }




        
    }
}
