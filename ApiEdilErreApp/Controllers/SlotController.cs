using ApiEdilErreApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace ApiEdilErreApp.Controllers;

[ApiController]
[Route("[controller]")]
public class SlotController : ControllerBase
{

    public class DTOSlot(){

            public int IdSlot { get; set; }
            public int? IdCantiere { get; set; }
            public int? IdMacroFase { get; set; }
            public int? IdOperatore { get; set; }
            public int? IdTipoSlot { get; set; }
            public string CodiceCantiereMacroFase { get; set; }
            public string CodiceTipoSlot { get; set; }
            public string NomePeratore { get; set; }
            public string CognomeOperatore { get; set; }
            public DateTime? DataSlot { get; set; }
            public string DenominazioneCantiere { get; set; }
            public string DescrizioneCantiereMacroFase { get; set; }
            public string DescrizioneTipoSlot { get; set; }
            
            public int? OrdinaleCantiereMacroFase { get; set; }
            public List<TabAutoMezzi> SlotAutomezzi { get; set; }
            public int? ViaggiDiscarica { get; set; }
            public int? ViaggiMateriali { get; set; }
   
    }


    [HttpGet(Name = "GetSlot")]
    public ActionResult<DTOSlot> Get(int idSlot)
    {

        using(var db = new CoraziendaSLContext()){

            try
            {
                DTOSlot tmp = new DTOSlot();
                TabAPP_Slots row = db.TabAPP_Slots.Where(x => x.APP_IdSlot == idSlot).FirstOrDefault();

                tmp.IdSlot = row.APP_IdSlot;
                tmp.IdCantiere = row.APP_IdCantiere;
                tmp.IdMacroFase = row.APP_IdMacroFase;
                tmp.IdOperatore = row.APP_IdOperatore;
                tmp.IdTipoSlot = row.APP_IdTipoSlot;
                tmp.CodiceCantiereMacroFase = db.TabCantieriMacroFasi.Where(x => x.IdCantiereMacroFase == row.APP_IdMacroFase).FirstOrDefault().CodiceCantiereMacroFase;
                tmp.CodiceTipoSlot = db.TabAPP_TipoSlot.Where(x => x.APP_IdTipoSlot == row.APP_IdTipoSlot).FirstOrDefault().APP_CodiceTipoSlot;
                tmp.NomePeratore = db.TabAPP_Operatori.Where(x => x.APP_IdOperatore == row.APP_IdOperatore).FirstOrDefault().APP_NomePeratore;
                tmp.CognomeOperatore = db.TabAPP_Operatori.Where(x => x.APP_IdOperatore == row.APP_IdOperatore).FirstOrDefault().APP_CognomeOperatore;
                tmp.DataSlot = row.APP_DataSlot;
                tmp.DenominazioneCantiere = db.TabCantieri.Where(x => x.IdCantiere == row.APP_IdCantiere).FirstOrDefault().DenominazioneCantiere;
                tmp.DescrizioneCantiereMacroFase = db.TabCantieriMacroFasi.Where(x => x.IdCantiereMacroFase == row.APP_IdMacroFase).FirstOrDefault().DescrizioneCantiereMacroFase;
                tmp.DescrizioneTipoSlot = db.TabAPP_TipoSlot.Where(x => x.APP_IdTipoSlot == row.APP_IdTipoSlot).FirstOrDefault().APP_DescrizioneTipoSlot;
                tmp.OrdinaleCantiereMacroFase = db.TabCantieriMacroFasi.Where(x => x.IdCantiereMacroFase == row.APP_IdMacroFase).FirstOrDefault().OrdinaleCantiereMacroFase;


                tmp.SlotAutomezzi = new List<TabAutoMezzi>();

                List<TabAPP_Automezzi> listaMoltiAMolti = db.TabAPP_Automezzi.Where(x => x.APP_IdSlot == row.APP_IdSlot).ToList();

                foreach (var item in listaMoltiAMolti)
                {

                    TabAutoMezzi tmpAutoMezzo = db.TabAutoMezzi.Where(x => x.IdAutoMezzo == item.IdAutoMezzo).FirstOrDefault();

                    if (tmpAutoMezzo != null)
                    {
                        tmp.SlotAutomezzi.Add(tmpAutoMezzo);
                    }

                }


                tmp.ViaggiDiscarica = db.TabAPP_Slots.Where(x => x.APP_IdSlot == row.APP_IdSlot).FirstOrDefault().APP_ViaggiDiscarica;
                tmp.ViaggiMateriali = db.TabAPP_Slots.Where(x => x.APP_IdSlot == row.APP_IdSlot).FirstOrDefault().App_ViaggiMateriali;

                return Ok(tmp);
            }
            catch(Exception e)
            {
                return NotFound("Errore: " + e.Message);
            }

            
        };
        
    }


    [HttpDelete(Name = "DeleteSlot")]
    public ActionResult<DTOSlot> Delete(int idSlot)
    {
        using (var db = new CoraziendaSLContext())
        {

            try
            {

                //Cancellazione dei mezzi associati allo slot

                List<TabAPP_Automezzi> listaMoltiAMolti = db.TabAPP_Automezzi.Where(x => x.APP_IdSlot == idSlot).ToList();

                foreach (var item in listaMoltiAMolti)
                {
                    db.TabAPP_Automezzi.Remove(item);
                    db.SaveChanges();
                }
                
                //Cancellazione dello slot
                db.TabAPP_Slots.Remove(db.TabAPP_Slots.Where(x => x.APP_IdSlot == idSlot).FirstOrDefault());
                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return NotFound("Errore: " + e.Message);
            }
        }
    }


    [HttpPost(Name = "PostSlot")]
    public ActionResult<DTOSlot> Post(DTOSlot slot)
    {
        using (var db = new CoraziendaSLContext())
        {
            try
            {
                //Insert dello slot

                TabAPP_Slots tmp = new TabAPP_Slots();
                tmp.APP_DataSlot = slot.DataSlot;
                tmp.APP_IdCantiere = slot.IdCantiere;
                tmp.APP_IdMacroFase = slot.IdMacroFase;
                tmp.APP_IdOperatore = slot.IdOperatore;
                tmp.APP_IdTipoSlot = slot.IdTipoSlot;
                tmp.APP_ViaggiDiscarica = slot.ViaggiDiscarica;
                tmp.App_ViaggiMateriali = slot.ViaggiMateriali;


                db.TabAPP_Slots.Add(tmp);
                db.SaveChanges();

                slot.IdSlot = db.TabAPP_Slots.OrderByDescending(x => x.APP_IdSlot).FirstOrDefault().APP_IdSlot;


                //Insert dei mezzi associati allo slot
                foreach(TabAutoMezzi mezzo in slot.SlotAutomezzi)
                {
                    TabAPP_Automezzi tmpMoltiAMolti = new TabAPP_Automezzi();
                    tmpMoltiAMolti.APP_IdSlot = slot.IdSlot;
                    tmpMoltiAMolti.IdAutoMezzo = mezzo.IdAutoMezzo;

                    db.TabAPP_Automezzi.Add(tmpMoltiAMolti);
                    db.SaveChanges();
                }


                return Ok(slot);
            }
            catch (Exception e)
            {
                return NotFound("Errore: " + e.Message);
            }
        }
    }


}
