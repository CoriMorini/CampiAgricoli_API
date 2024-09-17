using ApiEdilErreApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class CantieriController : ControllerBase
{
    [HttpGet(Name = "GetCantieri")]
    public ActionResult<List<TabCantieri>> Get()
    {
        using(var db = new CoraziendaSLContext()){
            
            try
            {
                return Ok(db.TabCantieri.Where(x => x.IdCantiere > 0).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
            
        };
        
    }
}
