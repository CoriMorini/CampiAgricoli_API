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
        public class NutrientScoreData
        {
            public double? ValoreMedio { get; set; }
            public int Punteggio { get; set; }
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

        /// <summary>
        /// Endpoint per ottenere il punteggio NScore basato sul valore di N (Azoto) per un campo specifico
        /// </summary>
        /// <param name="idCampo">ID del campo agricolo</param>
        /// <returns>Punteggio NScore per il campo agricolo</returns>
        [HttpGet("GetNScore")]
        public ActionResult<NutrientScoreData> GetNScore(int idCampo)
        {
            using (var db = new CampiAgricoliContext())
            {
                List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi
                    .Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 1) // IdTipologiaSensore per azoto
                    .ToList();

                if (misurazioniCampo.Count == 0)
                {
                    return Ok(new NutrientScoreData { ValoreMedio = 0, Punteggio = 0 });
                }

                double valoreMedioN = (double)misurazioniCampo.Average(x => x.valoreMisurazione);
                int NScore = CalcolaPunteggioSalute(valoreMedioN, ValoreIdealeN);

                return Ok(new NutrientScoreData
                {
                    ValoreMedio = valoreMedioN,
                    Punteggio = NScore
                });
            }
        }

        /// <summary>
        /// Endpoint per ottenere il punteggio FScore basato sul valore di fosforo per un campo specifico
        /// </summary>
        /// <param name="idCampo">ID del campo agricolo</param>
        /// <returns>Punteggio FScore per il campo agricolo</returns>
        [HttpGet("GetFScore")]
        public ActionResult<NutrientScoreData> GetFScore(int idCampo)
        {
            using (var db = new CampiAgricoliContext())
            {
                List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi
                    .Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 2) // IdTipologiaSensore per fosforo
                    .ToList();

                if (misurazioniCampo.Count == 0)
                {
                    return Ok(new NutrientScoreData { ValoreMedio = 0, Punteggio = 0 });
                }

                double valoreMedioFosforo = (double)misurazioniCampo.Average(x => x.valoreMisurazione);
                int FScore = CalcolaPunteggioSalute(valoreMedioFosforo, ValoreIdealeFosforo);

                return Ok(new NutrientScoreData
                {
                    ValoreMedio = valoreMedioFosforo,
                    Punteggio = FScore
                });
            }
        }

        /// <summary>
        /// Endpoint per ottenere il punteggio KScore basato sul valore di potassio per un campo specifico
        /// </summary>
        /// <param name="idCampo">ID del campo agricolo</param>
        /// <returns>Punteggio KScore per il campo agricolo</returns>
        [HttpGet("GetKScore")]
        public ActionResult<NutrientScoreData> GetKScore(int idCampo)
        {
            using (var db = new CampiAgricoliContext())
            {
                List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi
                    .Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 3) // IdTipologiaSensore per potassio
                    .ToList();

                if (misurazioniCampo.Count == 0)
                {
                    return Ok(new NutrientScoreData { ValoreMedio = 0, Punteggio = 0 });
                }

                double valoreMedioPotassio = (double)misurazioniCampo.Average(x => x.valoreMisurazione);
                int KScore = CalcolaPunteggioSalute(valoreMedioPotassio, ValoreIdealePotassio);

                return Ok(new NutrientScoreData
                {
                    ValoreMedio = valoreMedioPotassio,
                    Punteggio = KScore
                });
            }
        }

        /// <summary>
        /// Endpoint per ottenere il punteggio HScore basato sull'umidità per un campo specifico
        /// </summary>
        /// <param name="idCampo">ID del campo agricolo</param>
        /// <returns>Punteggio HScore per il campo agricolo</returns>
        [HttpGet("GetHScore")]
        public ActionResult<NutrientScoreData> GetHScore(int idCampo)
        {
            using (var db = new CampiAgricoliContext())
            {
                List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi
                    .Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 4) // IdTipologiaSensore per umidità
                    .ToList();

                if (misurazioniCampo.Count == 0)
                {
                    return Ok(new NutrientScoreData { ValoreMedio = 0, Punteggio = 0 });
                }

                double valoreMedioUmidita = (double)misurazioniCampo.Average(x => x.valoreMisurazione);
                int HScore = CalcolaPunteggioSalute(valoreMedioUmidita, ValoreIdealeUmidita);

                return Ok(new NutrientScoreData
                {
                    ValoreMedio = valoreMedioUmidita,
                    Punteggio = HScore
                });
            }
        }

        /// <summary>
        /// Endpoint per ottenere il punteggio TempAmbienteScore basato sulla temperatura ambiente per un campo specifico
        /// </summary>
        /// <param name="idCampo">ID del campo agricolo</param>
        /// <returns>Punteggio TempAmbienteScore per il campo agricolo</returns>
        [HttpGet("GetTempAmbienteScore")]
        public ActionResult<NutrientScoreData> GetTempAmbienteScore(int idCampo)
        {
            using (var db = new CampiAgricoliContext())
            {
                List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi
                    .Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 5) // IdTipologiaSensore per temperatura ambiente
                    .ToList();

                if (misurazioniCampo.Count == 0)
                {
                    return Ok(new NutrientScoreData { ValoreMedio = 0, Punteggio = 0 });
                }

                double valoreMedioTempAmbiente = (double)misurazioniCampo.Average(x => x.valoreMisurazione);
                int TempAmbienteScore = CalcolaPunteggioSalute(valoreMedioTempAmbiente, ValoreIdealeTempAmbiente);

                return Ok(new NutrientScoreData
                {
                    ValoreMedio = valoreMedioTempAmbiente,
                    Punteggio = TempAmbienteScore
                });
            }
        }

        /// <summary>
        /// Endpoint per ottenere il punteggio TempSuoloScore basato sulla temperatura del suolo per un campo specifico
        /// </summary>
        /// <param name="idCampo">ID del campo agricolo</param>
        /// <returns>Punteggio TempSuoloScore per il campo agricolo</returns>
        [HttpGet("GetTempSuoloScore")]
        public ActionResult<NutrientScoreData> GetTempSuoloScore(int idCampo)
        {
            using (var db = new CampiAgricoliContext())
            {
                List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi
                    .Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 6) // IdTipologiaSensore per temperatura del suolo
                    .ToList();

                if (misurazioniCampo.Count == 0)
                {
                    return Ok(new NutrientScoreData { ValoreMedio = 0, Punteggio = 0 });
                }

                double valoreMedioTempSuolo = (double)misurazioniCampo.Average(x => x.valoreMisurazione);
                int TempSuoloScore = CalcolaPunteggioSalute(valoreMedioTempSuolo, ValoreIdealeTempSuolo);

                return Ok(new NutrientScoreData
                {
                    ValoreMedio = valoreMedioTempSuolo,
                    Punteggio = TempSuoloScore
                });
            }
        }
    }
}
