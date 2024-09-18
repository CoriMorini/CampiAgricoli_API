using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{
    #region data models di appoggio

    public class NPK
    {
        public double? N { get; set; }
        public double? P { get; set; }
        public double? K { get; set; }

        public DateTime dataOraCerta { get; set; }

    }

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



    #endregion


    [HttpGet("GetLastNPK")]
    public ActionResult<List<NPK>> GetLastNPK(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi.Where(x => x.IdCampo == idCampo).ToList();

            if (misurazioniCampo.Count == 0)
            {
                return NotFound("Non ci sono misurazioni per questo campo");
            }

            List<NPK> nPKs = new List<NPK>();

            for (int i = 0; i < 5; i++)
            {
                NPK nPK = new NPK();

                // N
                //
                foreach (var misurazione in misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 1).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5))
                {
                    nPK.N = misurazione.valoreMisurazione;
                    nPK.dataOraCerta = misurazione.dataOraCertaMisurazione;
                }

                // P
                //
                foreach (var misurazione in misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 2).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5))
                {
                    nPK.P = misurazione.valoreMisurazione;
                }

                // K
                //
                foreach (var misurazione in misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 3).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5))
                {
                    nPK.K = misurazione.valoreMisurazione;
                }

                


                //Add to list
                //
                nPKs.Add(nPK);
            }

            
            return Ok(nPKs); 

        };

    }


    [HttpGet("GetLastUmidita")]
    public ActionResult<List<Um>> GetLastUmidita(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi.Where(x => x.IdCampo == idCampo).ToList();

            List<Um> umidita = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 4).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Um { Umidita = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            return Ok(umidita);
        };

    }

    [HttpGet("GetLastTemperaturaAmb")]
    public ActionResult<List<Temp>> GetLastTemperaturaAmb(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi.Where(x => x.IdCampo == idCampo).ToList();

            List<Temp> temp = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 5).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Temp { Temperatura = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            return Ok(temp);
        };

    }

    [HttpGet("GetLastTemperaturaSuolo")]
    public ActionResult<List<Temp>> GetLastTemperaturaSuolo(int idCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            List<VistaMisurazioniCampi> misurazioniCampo = db.VistaMisurazioniCampi.Where(x => x.IdCampo == idCampo).ToList();

            List<Temp> temp = misurazioniCampo.Where(x => x.IdCampo == idCampo && x.IdTipologiaSensore == 6).OrderByDescending(x => x.dataOraCertaMisurazione).Take(5).Select(x => new Temp { Temperatura = x.valoreMisurazione, dataOraCerta = x.dataOraCertaMisurazione }).ToList();

            return Ok(temp);
        };

    }




}
