using ApiCampiAgricoli.Models;
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



    [HttpGet("GetLogin")]
    public ActionResult<TabUtenti> Get(String username, String password)
    {
        using (var db = new CampiAgricoliContext())
        {
            TabUtenti? operatore = db.TabUtenti.Where(x => x.UsernameUtente == username && x.PasswordUtente == password).FirstOrDefault();
            
            if(operatore == null)
            {
                return NotFound("Operatore non trovato");
            }

            

            return Ok(operatore);

        };

    }

    
}
