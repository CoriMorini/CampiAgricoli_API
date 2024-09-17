using ApiEdilErreApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AppvistaSlotsController : ControllerBase
{



    [HttpGet(Name = "GetAppVistaSlots")]
    public ActionResult<List<APPVistaSlot>> Get(int idOperatore, DateTime data)
    {
        using (var db = new CoraziendaSLContext())
        {

            try
            {
                return Ok(db.APPVistaSlot.Where(x => x.APP_IdOperatore == idOperatore && x.APP_DataSlot == data).OrderBy(x => x.APP_IdSlot).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        };
        
    }
}
