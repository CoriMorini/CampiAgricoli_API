using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiEdilErreApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrendController : ControllerBase
    {
        #region data models e funzioni di appoggio

        public class MisurazioneAnnuale
        {
            public string mese { get; set; }
            public double valore { get; set; }
        }

        // Modello per il punteggio dei vari nutrienti e condizioni
        public class DataInfoTrend
        {
            public int PunteggioSalute { get; set; }

            public List<MisurazioneAnnuale> misurazioniAnnuali { get; set; }
        }

        // Funzione generica per calcolare il punteggio basato su valori
        private static int CalcolaPunteggioSalute(double valoreMedio, int indiceValoreIdeale)
        {
            // Lista dei valori ideali per i nutrienti e le condizioni
            List<double> ValoriIdeali = new List<double>
            {
                200, // Valore ideale per il azoto
                60,  // Valore ideale per il fosforo
                300, // Valore ideale per il potassio
                50,  // Valore ideale per l'umidità (in %)
                25,  // Valore ideale per la temperatura ambiente (in °C)
                20   // Valore ideale per la temperatura del suolo (in °C)
            };

        // Calcola la percentuale di valore medio rispetto al valore ideale
        double punteggio = (valoreMedio / ValoriIdeali[indiceValoreIdeale - 1]) * 100;

            // Assicurati che il punteggio non superi 100
            return (int)Math.Min(punteggio, 100);
        }

        


        #endregion


        [HttpGet("GetTrendGenerale")]
        public ActionResult<List<DataInfoTrend>> GetTrendGenerale(int idCampo)
        {
            List<DataInfoTrend> risultati = new List<DataInfoTrend>();

            using (var db = new CampiAgricoliContext())
            {
                try
                {
                    for(int j = 1; j <= 4; j++)
                    {
                        List<VistaMisurazioniUtente> listaMisurazioniCampi = db.VistaMisurazioniUtente.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == j).ToList();

                        double media = listaMisurazioniCampi.Average(x => x.valoreMisurazione) ?? 0;

                        int punteggio = CalcolaPunteggioSalute(media, j);

                        List<MisurazioneAnnuale> misurazioniAnnuali = new List<MisurazioneAnnuale>();

                        for (int i = 1; i <= 12; i++)
                        {
                            MisurazioneAnnuale tmp = new MisurazioneAnnuale();

                            tmp.mese = new DateTime(2021, i, 1).ToString("MMM");
                            tmp.valore = listaMisurazioniCampi.Where(x => x.dataOraCertaMisurazione.Month == i).Average(x => x.valoreMisurazione) ?? 0;

                            misurazioniAnnuali.Add(tmp);
                        }


                        risultati.Add(new DataInfoTrend
                        {
                            PunteggioSalute = punteggio,
                            misurazioniAnnuali = misurazioniAnnuali

                        });
                    }

                }
                catch (Exception e)
                {
                    return NotFound(e.Message);
                }
            }

            return Ok(risultati);
        }

    }

}
