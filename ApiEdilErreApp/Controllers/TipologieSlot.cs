using ApiEdilErreApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class TipologieSlot : ControllerBase
{
    [HttpGet(Name = "GetTipologieSlot")]
    public ActionResult<List<TabAPP_TipoSlot>> Get()
    {
        using(var db = new CoraziendaSLContext()){

           

            try
            {
                return Ok(db.TabAPP_TipoSlot.Where(x => x.APP_IdTipoSlot > 0).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        };
        
    }
}
