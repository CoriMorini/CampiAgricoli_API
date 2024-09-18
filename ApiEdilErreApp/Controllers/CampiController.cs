using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class CampiController : ControllerBase
{
    



    [HttpGet("GetCampi")]
    public ActionResult<TabUtenti> Get(int idUtente)
    {
        using (var db = new CampiAgricoliContext())
        {
            //Prendo tutti i campi dell'utente
            //
            List<TabCampi> campi = db.TabCampi.Where(x => x.IdUtente == idUtente).ToList();
            

            if(campi.Count == 0l)
            {
                return NotFound("Questo utente non ha campi.");
            }

            return Ok(campi);

        };

    }

    
}
