using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class Microcontrollori : ControllerBase
{
    

    [HttpGet("GetMicrontrollori")]
    public ActionResult<TabUtenti> Get(string filtro, int numeroPagina)
    {
        using (var db = new CampiAgricoliContext())
        {
           List<TabMicrocontrollori> listaMicrocontrollori = db.TabMicrocontrollori
                .Where(x => x.NomeMicrocontrollore.Contains(filtro) || x.IdMicrocontrollore == Convert.ToInt64(filtro) || x.IdCampo == Convert.ToInt64(filtro))
                .Skip(10 * numeroPagina)
                .Take(10).ToList();

            if (listaMicrocontrollori.Count == 0)
            {
                return NotFound("Nessun microcontrollori trovato.");
            }

            return Ok(listaMicrocontrollori);

        };

    }

    
}
