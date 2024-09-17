using ApiEdilErreApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class MacroFasiController : ControllerBase
{
    [HttpGet(Name = "GetFasiCantiere")]
    public ActionResult<List<TabCantieriMacroFasi>> Get()
    {
        using(var db = new CoraziendaSLContext()){

            try
            {
                return Ok(db.TabCantieriMacroFasi.Where(x => x.IdCantiereMacroFase > 0).ToList());
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }

            
        };
        
    }
}
