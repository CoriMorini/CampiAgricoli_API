using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class UtilityController : ControllerBase
{
    
    [HttpGet("AddMisurazioni")]
    public ActionResult<int> Get(int idUtente, int numeroMisurazioni)
    {
        int misurazioniInserite = 0;

        using (var db = new CampiAgricoliContext())
        {
            //Prendo i campi dell'utente
            List<TabCampi> campi = db.TabCampi.Where(x => x.IdUtente == idUtente).ToList();

            foreach (TabCampi item in campi)
            {
                // Prendo i microncontrollori del campo
                List<TabMicrocontrollori> microcontrollori = db.TabMicrocontrollori.Where(x => x.IdCampo == item.IdCampo).ToList();

                foreach (TabMicrocontrollori micro in microcontrollori)
                {
                    // Prendo i sensori del microcontrollore
                    List<TabSensori> sensori = db.TabSensori.Where(x => x.IdMicrocontrollore == micro.IdMicrocontrollore).ToList();

                    foreach (TabSensori sensore in sensori)
                    {

                        for (int i = 0; i < numeroMisurazioni; i++)
                        {
                            TabMisurazioni tmp = new TabMisurazioni();

                            tmp.IdSensore = sensore.IdSensore;
                            tmp.valoreMisurazione = new Random().Next(0, 100);
                            tmp.dataOraCertaMisurazione = new DateTime(new Random().Next(2020, DateTime.Now.Year), new Random().Next(1, 13), new Random().Next(1, 29), new Random().Next(0, 24), new Random().Next(0, 60), new Random().Next(0, 60));

                            db.TabMisurazioni.Add(tmp);

                            misurazioniInserite++;
                        }
                    }
                }
            }

            db.SaveChanges();
        };

        return misurazioniInserite;
    }

    
}
