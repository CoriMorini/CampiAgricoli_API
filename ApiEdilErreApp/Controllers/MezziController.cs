using ApiEdilErreApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class MezziController : ControllerBase
{
    [HttpGet(Name = "GetMezzi")]
    public ActionResult<List<TabAutoMezzi>> Get()
    {
        using(var db = new CoraziendaSLContext()){

            try
            {
                return Ok(db.TabAutoMezzi.Where(x => x.IdAutoMezzo > 0).ToList());
            }
            catch (Exception ex){
                return BadRequest(ex.Message);
            }

            
        };
        
    }
}
