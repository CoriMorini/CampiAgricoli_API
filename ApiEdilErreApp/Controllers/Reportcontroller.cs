using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{
    #region data models di appoggio

    public class Um
    {
        public double? Umidita { get; set; }
        public DateTime dataOraCerta { get; set; }
    }

    public class Temp
    {
        public double? Temperatura { get; set; }
        public DateTime dataOraCerta { get; set; }
    }

    public class ReportGenerale 
    {         
        public List<Um> umiditaAmb { get; set; }
        public List<Um> umiditaTer { get; set; }
        public List<Temp> temperaturaAmb { get; set; }
        public List<Temp> temperaturaTer { get; set; }

        public ReportGenerale()
        {
            umiditaAmb = new List<Um>();
            umiditaTer = new List<Um>();
            temperaturaAmb = new List<Temp>();
            temperaturaTer = new List<Temp>();
        }
    }



    #endregion

    

    [HttpGet("GetLastUmiditaAmb")]
    public ActionResult<List<Um>> GetLastUmiditaAmb(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniUtente> misurazioniCampo = db.VistaMisurazioniUtente.Where(x => x.IdCampo == idCampo).ToList();

            List<Um> umidita = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 1).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Um { Umidita = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            return Ok(umidita);
        };

    }

    [HttpGet("GetLastUmiditaTer")]
    public ActionResult<List<Um>> GetLastUmiditaTer(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniUtente> misurazioniCampo = db.VistaMisurazioniUtente.Where(x => x.IdCampo == idCampo).ToList();

            List<Um> umidita = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 2).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Um { Umidita = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            return Ok(umidita);
        };

    }

    [HttpGet("GetLastTemperaturaAmb")]
    public ActionResult<List<Temp>> GetLastTemperaturaAmb(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniUtente> misurazioniCampo = db.VistaMisurazioniUtente.Where(x => x.IdCampo == idCampo).ToList();

            List<Temp> temp = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 3).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Temp { Temperatura = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            return Ok(temp);
        };

    }

    [HttpGet("GetLastTemperaturaTer")]
    public ActionResult<List<Temp>> GetLastTemperaturaTer(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniUtente> misurazioniCampo = db.VistaMisurazioniUtente.Where(x => x.IdCampo == idCampo).ToList();

            List<Temp> temp = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 4).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Temp { Temperatura = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            return Ok(temp);
        };

    }

    [HttpGet("GetReportGenerale")]
    public ActionResult<ReportGenerale> GetReportGenerale(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            ReportGenerale report = new ReportGenerale();

            List<VistaMisurazioniUtente> misurazioniCampo = db.VistaMisurazioniUtente.Where(x => x.IdCampo == idCampo).ToList();

            #region UmiditaAmb

            report.umiditaAmb = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 1).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Um { Umidita = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            #endregion

            #region UmiditaTer

            report.umiditaTer = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 2).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Um { Umidita = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            #endregion

            #region TemperaturaAmb

            report.temperaturaAmb = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 3).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Temp { Temperatura = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            #endregion

            #region TemperaturaSuolo

            report.temperaturaTer = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 4).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Temp { Temperatura = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            #endregion

            return Ok(report);
        };

    }


}
