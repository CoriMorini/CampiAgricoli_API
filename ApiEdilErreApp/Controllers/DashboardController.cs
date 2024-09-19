using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class DashboardController : ControllerBase
{

    #region data models di appoggio

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


    public class TemperaturaMediaMese 
    {
        public string mese { get; set; }
        public double temperatura { get; set; }
    }



    #endregion


    [HttpGet("GetListCardCampi")]
    public ActionResult<bool> GetListCardCampi(int idUtente)
    {
        using (var db = new CampiAgricoliContext())
        {
            return true;

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
