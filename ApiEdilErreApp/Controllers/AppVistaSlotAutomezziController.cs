using ApiEdilErreApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AppvistaSlotAutomezziController : ControllerBase
{
    [HttpGet(Name = "GetAppvistaSlotAutomezzi")]
    public ActionResult<List<APPVistaSlotAutomezzi>> Get()
    {
        using(var db = new CoraziendaSLContext()){

            try
            {
                return Ok(db.APPVistaSlotAutomezzi.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            
        };
        
    }
}
