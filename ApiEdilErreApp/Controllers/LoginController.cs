using ApiEdilErreApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    public class User()
    {
        public int IdOperatore { get; set; }
        public String NomeOperatore { get; set; }
        public String CognomeOperatore { get; set; }

    }



    [HttpGet("auth")]
    public ActionResult<TabAPP_Operatori> Get(String username, String password)
    {
        using (var db = new CoraziendaSLContext())
        {
            TabAPP_Operatori operatore = db.TabAPP_Operatori.Where(x => x.APP_Uid == username && x.APP_Pwd == password).FirstOrDefault();
            
            if(operatore == null)
            {
                return NotFound("Operatore non trovato");
            }

            /*
            User user = new User
            {
                IdOperatore = operatore.APP_IdOperatore,
                NomeOperatore = operatore.APP_NomePeratore,
                CognomeOperatore = operatore.APP_CognomeOperatore
            };
            */

            return Ok(operatore);

        };

    }

    [HttpGet("check-validity")]
    public ActionResult<bool> Get(int idOperatore)
    {
        return Ok(true);
    }
}
