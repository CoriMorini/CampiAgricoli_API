using ApiCampiAgricoli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


    [HttpGet("GeneraNuovoCampo")]
    public ActionResult<int> Get(int idUtente, String nomeCampo)
    {
        using (var db = new CampiAgricoliContext())
        {
            TabCampi tmp = new TabCampi();

            tmp.IdUtente = idUtente;
            tmp.NomeCampo = nomeCampo;

            db.TabCampi.Add(tmp);
            db.SaveChanges();

            for (int i = 0; i < new Random().Next(1, 10); i++)
            {
                TabMicrocontrollori micro = new TabMicrocontrollori();

                micro.IdCampo = tmp.IdCampo;

                db.TabMicrocontrollori.Add(micro);
                db.SaveChanges();

                List<TabSensoriTipologie> tipologieSensori = db.TabSensoriTipologie.ToList();

                foreach(TabSensoriTipologie tipologia in tipologieSensori)
                {
                   
                    TabSensori sensore = new TabSensori();

                    sensore.IdMicrocontrollore = micro.IdMicrocontrollore;
                    sensore.IdTipologiaSensore = tipologia.IdTipologiaSensore; 

                    db.TabSensori.Add(sensore);
                    
                }
            }


            db.SaveChanges();

            return tmp.IdCampo;
        };
    }


    [HttpGet("GeneraDatabase")]
    public ActionResult<String> GeneraDatabase()
    {
        using (var db = new CampiAgricoliContext())
        {
            try
            {
                // Crea un'unica istanza di Random
                Random random = new Random();

                for (int i = 1; i < 3; i++)
                {
                    TabUtenti utente = new TabUtenti();

                    utente.UsernameUtente = "U" + i;
                    utente.PasswordUtente = "U" + i;
                    utente.NomeUtente = "Nome " + i;
                    utente.CognomeUtente = "Cognome " + i;


                    db.TabUtenti.Add(utente);

                    db.SaveChanges();

                    for (int j = 1; j < 6; j++)
                    {
                        TabCampi campo = new TabCampi();

                        campo.IdUtente = utente.IdUtente;
                        campo.NomeCampo = "Campo " + j;

                        db.TabCampi.Add(campo);
                        db.SaveChanges();

                        for (int k = 1; k < 4; k++)
                        {
                            TabMicrocontrollori micro = new TabMicrocontrollori();

                            micro.IdCampo = campo.IdCampo;
                            micro.NomeMicrocontrollore = "Micro " + k;
                            micro.Latitudine = random.Next(-90, 90);
                            micro.Longitudine = random.Next(-180, 180);

                            db.TabMicrocontrollori.Add(micro);
                            db.SaveChanges();

                            List<TabSensoriTipologie> tipologieSensori = db.TabSensoriTipologie.ToList();

                            foreach (TabSensoriTipologie tipologia in tipologieSensori)
                            {

                                TabSensori sensore = new TabSensori();

                                sensore.IdMicrocontrollore = micro.IdMicrocontrollore;
                                sensore.IdTipologiaSensore = tipologia.IdTipologiaSensore;

                                db.TabSensori.Add(sensore);

                            }

                            db.SaveChanges();


                        }
                    }

                }


                int count = 0;

                //Genera 24 misurazioni per sensore al giorno per 3 anni
                //
                foreach (TabSensori sensore in db.TabSensori.ToList())
                {
                    for (int l = 0; l < 3; l++)
                    {
                        for (int m = 1; m <= 12; m++) // i mesi vanno da 1 a 12
                        {
                            int giorniNelMese = DateTime.DaysInMonth(2023 + l, m); // Numero di giorni corretti nel mese

                            for (int n = 1; n <= giorniNelMese; n++) // i giorni vanno da 1 al numero di giorni del mese
                            {
                                //for (int o = 0; o < 12; o++) // le ore vanno da 0 a 23
                                //{
                                    TabMisurazioni misurazione = new TabMisurazioni();

                                    misurazione.IdSensore = sensore.IdSensore;
                                    misurazione.valoreMisurazione = random.Next(0, 100);
                                    misurazione.dataOraCertaMisurazione = new DateTime(2023 + l, m, n, 12, 0, 0); // Data valida

                                    db.TabMisurazioni.Add(misurazione);

                                    count++;
                                //}
                            }
                        }
                    }                 

                }

                db.SaveChanges();

                return Ok("Inserite " + count + " Misurazioni");


            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
 
        };

    }


    [HttpGet("ResetDatabase")]
    public ActionResult<String> Get()
    {
        using (var db = new CampiAgricoliContext())
        {
            try
            {
                // Disabilita temporaneamente le chiavi esterne
                db.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'");

                // Cancella i dati da tutte le tabelle
                foreach (var entity in db.Model.GetEntityTypes())
                {

                    if(entity.Name.Contains("Tipologie") || entity.Name.Contains("Vista"))
                    {
                        continue;
                    }

                    var tableName = entity.GetTableName();
                    db.Database.ExecuteSqlRaw($"DELETE FROM {tableName};");

                    // Reset ID auto-incrementali per ogni tabella
                    db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('{tableName}', RESEED, 0);");
                }

                // Riabilita le chiavi esterne
                db.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable @command1='ALTER TABLE ? CHECK CONSTRAINT ALL'");

                return Ok("Tutti i dati sono stati cancellati e gli ID sono stati resettati.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }


}
