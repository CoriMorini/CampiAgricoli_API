using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class Microcontrollori : ControllerBase
{


    [HttpGet("GetMicrocontrollori")]
    public ActionResult<List<VistaMicrocontrolloriUtente>> Get(int idUtente, int numeroPagina, string filtro = "")
    {
        using (var db = new CampiAgricoliContext())
        {
            var query = db.VistaMicrocontrolloriUtente.AsQueryable();

            query = query.Where(x => x.IdUtente == idUtente);

            if (!string.IsNullOrEmpty(filtro))
            {
                // Proviamo a fare il parsing del filtro in un long
                if (long.TryParse(filtro, out long filtroNumerico))
                {
                    query = query.Where(x => x.NomeMicrocontrollore.Contains(filtro) 
                    || x.NomeCampo.Contains(filtro) 
                    || x.IdMicrocontrollore == filtroNumerico 
                    || x.IdCampo == filtroNumerico
                    || x.Latitudine == filtroNumerico
                    || x.Longitudine == filtroNumerico);
                }
                else
                {
                    query = query.Where(x => x.NomeMicrocontrollore.Contains(filtro) || x.NomeCampo.Contains(filtro));
                }
            }

            List<VistaMicrocontrolloriUtente> listaMicrocontrollori = query
                .Skip(10 * numeroPagina)
                .Take(10).ToList();

            if (listaMicrocontrollori.Count == 0)
            {
                return NotFound("Nessun microcontrollore trovato.");
            }

            return Ok(listaMicrocontrollori);
        }
    }

    [HttpGet("GetNumeroPagine")]
    public ActionResult<int> GetNumeroPagine(int idUtente, string filtro = "")
    {
        using (var db = new CampiAgricoliContext())
        {
            var query = db.VistaMicrocontrolloriUtente.AsQueryable();

            query = query.Where(x => x.IdUtente == idUtente);

            if (!string.IsNullOrEmpty(filtro))
            {
                // Proviamo a fare il parsing del filtro in un long
                if (long.TryParse(filtro, out long filtroNumerico))
                {
                    query = query.Where(x => x.NomeMicrocontrollore.Contains(filtro)
                    || x.NomeCampo.Contains(filtro)
                    || x.IdMicrocontrollore == filtroNumerico
                    || x.IdCampo == filtroNumerico
                    || x.Latitudine == filtroNumerico
                    || x.Longitudine == filtroNumerico);

                }
                else
                {
                    query = query.Where(x => x.NomeMicrocontrollore.Contains(filtro) || x.NomeCampo.Contains(filtro));
                }
            }

            int numeroPagine = query.Count() / 10;

            if (query.Count() % 10 != 0)
            {
                numeroPagine++;
            }

            return Ok(numeroPagine);
        }
    }

    [HttpGet("GetMicrocontrollore")]
    public ActionResult<TabMicrocontrollori> GetMicrocontrollore(int idMicrocontrollore)
    {
        using (var db = new CampiAgricoliContext())
        {
            var microcontrollore = db.TabMicrocontrollori.FirstOrDefault(x => x.IdMicrocontrollore == idMicrocontrollore);

            if (microcontrollore == null)
            {
                return NotFound("Microcontrollore non trovato.");
            }

            return Ok(microcontrollore);
        }
    }


    [HttpPost("UpdateMicrocontrollore")]
    public ActionResult UpdateMicrocontrollore([FromBody] TabMicrocontrollori tmp)
    {
        using (var db = new CampiAgricoliContext())
        {
            TabMicrocontrollori microcontrollore = db.TabMicrocontrollori.FirstOrDefault(x => x.IdMicrocontrollore == tmp.IdMicrocontrollore);

            if (microcontrollore == null)
            {
                return NotFound("Microcontrollore non trovato.");
            }

            microcontrollore.NomeMicrocontrollore = tmp.NomeMicrocontrollore;
            microcontrollore.Latitudine = tmp.Latitudine;
            microcontrollore.Longitudine = tmp.Longitudine;

            db.SaveChanges();
        }

        return Ok();
    }

}
    