using ApiEdilErreApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class RendicontoMeseController : ControllerBase
{

    public class RendicontoGiorno(){

        public DateTime giorno {get; set;}
        public bool rendicontatoSiNo {get; set;}
        public bool rendicontabileSiNo {get; set;}
    }


    [HttpGet(Name = "GetRendicontoMese")]
    public ActionResult<List<RendicontoGiorno>> Get(int idOperatore, int mese, int anno)
    {
        using(var db = new CoraziendaSLContext()){
            
            
            try
            {
                List<RendicontoGiorno> res = new List<RendicontoGiorno>();

                foreach (var giorno in Enumerable.Range(1, DateTime.DaysInMonth(anno, mese)))
                {
                    RendicontoGiorno tmp = new RendicontoGiorno();

                    tmp.giorno = new DateTime(anno, mese, giorno);
                    tmp.rendicontatoSiNo = db.TabAPP_Slots.Where(x => x.APP_IdOperatore == idOperatore && x.APP_DataSlot == tmp.giorno).Count() > 0;

                    if(DateTime.Now.Month == mese && DateTime.Now.Year == anno)
                    {
                        tmp.rendicontabileSiNo = (giorno >= DateTime.Now.Day - db.TabAPP_Parametri.Where(x => x.APP_IdParametri == 1).First().APP_RendicontazioneMaxGG) && giorno <= DateTime.Now.Day;
                    }
                    else
                    {
                        tmp.rendicontabileSiNo = false;
                    }
                   

                    res.Add(tmp);
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            
                       
        };
        
    }
}
