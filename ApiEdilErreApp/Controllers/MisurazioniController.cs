using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.InMemory.Query.Internal;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class Misurazioni : ControllerBase
{



    [HttpPost("PostMisurazione")]
    public ActionResult UpdateMicrocontrollore([FromBody] List<TabMisurazioni> tmp)
    {
        using (var db = new CampiAgricoliContext())
        {
            try
            {

                foreach (TabMisurazioni item in tmp)
                {
                    item.dataOraCertaMisurazione = DateTime.Now;
                    db.TabMisurazioni.Add(item);
                }

                db.SaveChanges();
            }
            catch(Exception ex)
            {
                return NotFound(ex);
            }
            
        }

        return Ok();
    }

}
    