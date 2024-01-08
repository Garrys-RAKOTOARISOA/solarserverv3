using ASPAPI.Data;
using ASPAPI.Models;
using ASPAPI.Service;
using Microsoft.AspNetCore.Mvc;
namespace ASPAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarDataController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SolarDataController> _logger;
    
    public SolarDataController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("insertBatteryData/{idmodule}/{tension}/{energie}/{courant}", Name = "insertBatteryData")]
    public IActionResult insertBatteryData(int idmodule, double tension, double energie, double courant)
    {
        DureeUtilisationBatterie u = new DureeUtilisationBatterie()
        {
            IdModule = idmodule,
            Duree = 0,
            Date = MainService.getDateNow()
        };
        
        bool entryExists = _context.DureeUtilisationBatterie.Any(e => e.IdModule == u.IdModule && e.Date == u.Date);

        if (entryExists)
        {
            if (courant > 0)
            {
                DureeUtilisationBatterie duree = _context.DureeUtilisationBatterie
                    .First(a => a.IdModule == idmodule && a.Date == MainService.getDateNow());
                duree.Duree++;
                _context.SaveChanges();   
            }
        }
        else
        {
            _context.Add(u);
            _context.SaveChanges();
        }
        
        ModuleSolar moduleSolar = _context.Module.First(a => a.Id == idmodule);
        Batterie batterie = _context.Batterie.First(a => a.Id == moduleSolar.IdBatterie);
        BatterieData b = new BatterieData()
        {
            IdModule = idmodule,
            Tension = tension,
            Energie = energie,
            Courant = courant,
            Pourcentage = (tension * 100) / batterie.Valeur,
            Temps = (DateTime.Now).ToUniversalTime()
        };

        CouleurBoutonBatterie c = _context.CouleurBoutonBatterie.First(a => a.IdModule == idmodule);
        if (c == null)
        {
            CouleurBoutonBatterie newcouleur = new CouleurBoutonBatterie()
            {
                IdModule = idmodule,
                Couleur = "vert"
            };
            _context.Add(newcouleur);
            _context.SaveChanges();
        }
            
        if (courant <= 0)
        {
            c.Couleur = "rouge";
        }
        else
        {
            c.Couleur = "vert";
        }
        _context.Add(b);
        _context.SaveChanges();
        
        DateOnly today = MainService.getDateNow();

        List<PlanningBatterie> listeplanning = _context.PlanningBatterie
            .Where(a => a.IdModule == idmodule && 
                        today.CompareTo(a.DateDebut.Date) >= 0 &&
                        today.CompareTo(a.DateFin.Date) <= 0)
            .OrderBy(a => a.Id)
            .ToList();

        foreach (var v in listeplanning)
        {
            if (!v.Done)
            {
                if (v.DateFin <= DateTime.Now.ToUniversalTime())
                {
                    SwitchRelaisBatterie(idmodule);
                    v.Done = true;
                }

                if (energie >= v.ValeurEnergie)
                {
                    SwitchRelaisBatterie(idmodule);
                }
                v.Done = true;
                _context.SaveChanges();
            }
        }

        _context.SaveChanges();
        
        return Ok($"success battery data, time = "+DateTime.Now);
    }

    [HttpGet("insertPriseData/{idmodule}/{consommation}/{tension}/{puissance}/{courant}", Name = "insertPriseData")]
    public IActionResult insertPriseData(int idmodule, double consommation, double tension, double puissance,
        double courant)
    {
        TotalConsommationPrise u = new TotalConsommationPrise()
        {
            IdModule = idmodule,
            Total = 0,
            Date = MainService.getDateNow()
        };
        
        bool entryExists = _context.TotalConsommationPrise.Any(e => e.IdModule == u.IdModule && e.Date == u.Date);
        
        if (entryExists)
        {
            TotalConsommationPrise total = _context.TotalConsommationPrise
                .First(a => a.IdModule == idmodule && a.Date == MainService.getDateNow());
            total.Total += consommation;
            _context.SaveChanges();
        }
        else
        {
            _context.Add(u);
            _context.SaveChanges();
        }
        
        PriseData p = new PriseData()
        {
            IdModule = idmodule,
            Consommation = consommation,
            Tension = tension,
            Puissance = puissance,
            Courant = courant,
            Temps = (DateTime.Now).ToUniversalTime()
        };
        _context.Add(p);

        CouleurBoutonPrise c = _context.CouleurBoutonPrise
            .First(a => a.IdModule == idmodule);

        if (c == null)
        {
            CouleurBoutonPrise newc = new CouleurBoutonPrise()
            {
                IdModule = idmodule,
                Couleur = "vert"
            };
            _context.Add(newc);
            _context.SaveChanges();
        }

        if (tension <= 0)
        {
            c.Couleur = "rouge";
        }
        else
        {
            c.Couleur = "vert";
        }
        
        _context.SaveChanges();
        
        DateOnly today = MainService.getDateNow();

        List<PlanningPrise> listeplanning = _context.PlanningPrise
            .Where(a => a.IdModule == idmodule && 
                        today.CompareTo(a.DateDebut.Date) >= 0 &&
                        today.CompareTo(a.DateFin.Date) <= 0)
            .OrderBy(a => a.Id)
            .ToList();

        foreach (var v in listeplanning)
        {
            if (!v.Done)
            {
                if (v.DateFin <= DateTime.Now.ToUniversalTime())
                {
                    SwitchRelaisPrise(idmodule);
                    v.Done = true;
                }

                if (consommation >= v.ValeurConsommation)
                {
                    SwitchRelaisPrise(idmodule);
                    v.Done = true;
                }

                _context.SaveChanges();
            }
        }

        _context.SaveChanges();
        return Ok($"success prise data, time = "+DateTime.Now);
    }
    
    [HttpGet("insertPanneauData/{idmodule}/{production}/{tension}/{puissance}/{courant}", Name = "insertPanneauData")]
    public IActionResult insertPanneauData(int idmodule, double production, double tension, double puissance,
        double courant)
    {
        PanneauData p = new PanneauData()
        {
            IdModule = idmodule,
            Production = production,
            Tension = tension,
            Puissance = puissance,
            Courant = courant,
            Temps = (DateTime.Now).ToUniversalTime()
        };
        _context.Add(p);
        _context.SaveChanges();
        return Ok($"success panneau data, time = "+DateTime.Now);
    }

    [HttpGet("GetBatteryDataWithDate/{idmodule}/{jour}", Name = "GetBatteryDataWithDate")]
    public List<BatterieData> GetBatteryDataWithDate(int idmodule, DateOnly jour)
    {
        return _context.BatterieData
            .Where(a => a.IdModule == idmodule && a.Temps.Date.Equals(jour))
            .ToList();
    }

    [HttpGet("GetPriseDataWithDate/{idmodule}/{jour}", Name = "GetPriseDataWithDate")]
    public List<PriseData> GetPriseDataWithDate(int idmodule, DateOnly jour)
    {
        return _context.PriseData
            .Where(a => a.IdModule == idmodule && a.Temps.Date.Equals(jour))
            .ToList();
    }

    [HttpGet("GetSpecifiedConsommationPrise/{idmodule}/{jour}/{heuredebut}/{minutedebut}/{heurefin}/{minutefin}",
        Name = "GetSpecifiedConsommation")]
    public double GetSpecifiedConsommationPrise(int idmodule, DateOnly jour, int heuredebut, int minutedebut,
        int heurefin, int minutefin)
    {
        DateTime datedebut = new DateTime(jour.Year, jour.Month, jour.Day, heuredebut, minutedebut, 0);
        DateTime datefin = new DateTime(jour.Year, jour.Month, jour.Day, heurefin, minutefin, 0);

        PriseData debut = _context.PriseData.First(a => a.IdModule == idmodule && a.Temps == datedebut);
        PriseData fin = _context.PriseData.First(a => a.IdModule == idmodule && a.Temps == datefin);
        return fin.Consommation - debut.Consommation;
    }
    
    [HttpGet("GetSpecifiedProductionPanneau/{idmodule}/{jour}/{heuredebut}/{minutedebut}/{heurefin}/{minutefin}",
        Name = "GetSpecifiedProductionPanneau")]
    public double GetSpecifiedProductionPanneau(int idmodule, DateOnly jour, int heuredebut, int minutedebut,
        int heurefin, int minutefin)
    {
        DateTime datedebut = new DateTime(jour.Year, jour.Month, jour.Day, heuredebut, minutedebut, 0);
        DateTime datefin = new DateTime(jour.Year, jour.Month, jour.Day, heurefin, minutefin, 0);

        PanneauData debut = _context.PanneauData.First(a => a.IdModule == idmodule && a.Temps == datedebut);
        PanneauData fin = _context.PanneauData.First(a => a.IdModule == idmodule && a.Temps == datefin);
        return fin.Production - debut.Production;
    }
    
    [HttpGet("GetPanneauDataWithDate/{idmodule}/{jour}", Name = "GetPanneauDataWithDate")]
    public List<PanneauData> GetPanneauDataWithDate(int idmodule, DateOnly jour)
    {
        return _context.PanneauData
            .Where(a => a.IdModule == idmodule && a.Temps.Date.Equals(jour))
            .ToList();
    }
    
    [HttpGet("GetBattery", Name = "GetBattery")]
    public List<Batterie> GetBattery()
    {
        return _context.Batterie.ToList();
    }

    [HttpGet("ModuleById/{id}", Name = "ModuleById")]
    public ModuleSolar GetModule(int id)
    {
        return _context.Module.First(a => a.Id == id);
    }

    [HttpGet("ModuleByIdClient/{idclient}", Name = "ModuleByIdClient")]
    public ModuleSolar GetModuleByIdClient(int idclient)
    {
        Client client = _context.Client.First(a => a.Id == idclient);
        return _context.Module.First(a => a.Id == client.IdModule);
    }

    [HttpGet("SwitchRelaisPrise/{idmodule}", Name = "SwitchRelaisPrise")]
    public IActionResult SwitchRelaisPrise(int idmodule)
    {
        RelaisPrise relais = _context.RelaisPrise
            .First(a => a.IdModule == idmodule);
        if (relais.State)
        {
            NotificationModule notificationModule = new NotificationModule()
            {
                IdModule = idmodule,
                Texte = "La prise a ete eteint a " + (DateTime.Now).ToUniversalTime(),
                Temps = (DateTime.Now).ToUniversalTime()
            };
            _context.Add(notificationModule);
            relais.State = false;
        }
        else
        {
            NotificationModule notificationModule = new NotificationModule()
            {
                IdModule = idmodule,
                Texte = "La prise ete allumee a " + (DateTime.Now).ToUniversalTime(),
                Temps = (DateTime.Now).ToUniversalTime()
            };
            _context.Add(notificationModule);
            relais.State = true;
        }
        _context.SaveChanges();
        return Ok($"The relais prise has been switched at "+DateTime.Now);
    }
    
    [HttpGet("SwitchRelaisBatterie/{idmodule}", Name = "SwitchRelaisBatterie")]
    public IActionResult SwitchRelaisBatterie(int idmodule)
    {
        RelaisBatterie relais = _context.RelaisBatterie
            .First(a => a.IdModule == idmodule);
        if (relais.State)
        {
            NotificationModule notificationModule = new NotificationModule()
            {
                IdModule = idmodule,
                Texte = "La batterie a ete eteint a " + (DateTime.Now).ToUniversalTime(),
                Temps = (DateTime.Now).ToUniversalTime()
            };
            _context.Add(notificationModule);
            relais.State = false;
        }
        else
        {
            NotificationModule notificationModule = new NotificationModule()
            {
                IdModule = idmodule,
                Texte = "La batterie ete allumee a " + (DateTime.Now).ToUniversalTime(),
                Temps = (DateTime.Now).ToUniversalTime()
            };
            _context.Add(notificationModule);
            relais.State = true;
        }
        _context.SaveChanges();
        return Ok($"The relais battery has been switched at "+DateTime.Now);
    }

    [HttpGet("GetCouleurBoutonPrise/{idmodule}", Name = "GetCouleurBoutonPrise")]
    public CouleurBoutonPrise GetCouleurBoutonPrise(int idmodule)
    {
        return _context.CouleurBoutonPrise.First(a => a.IdModule == idmodule);
    }

    [HttpGet("GetCouleurBoutonBatterie/{idmodule}", Name = "GetCouleurBoutonBatterie")]
    public CouleurBoutonBatterie GetCouleurBoutonBatterie(int idmodule)
    {
        return _context.CouleurBoutonBatterie.First(a => a.IdModule == idmodule);
    }

    [HttpGet("GetListNotification/{idmodule}", Name = "GetListNotification")]
    public List<NotificationModule> GetListNotification(int idmodule)
    {
        return _context.NotificationModule
            .Where(a => a.IdModule == idmodule)
            .ToList();
    }
    
    [HttpGet("GetListTodayNotification/{idmodule}", Name = "GetListTodayNotification")]
    public List<NotificationModule> GetListTodayNotification(int idmodule)
    {
        return _context.NotificationModule
            .Where(a => a.IdModule == idmodule && a.Temps.Date.Equals(MainService.getDateNow()))
            .ToList();
    }

    [HttpGet("NotificationById/{id}", Name = "NotificationById")]
    public NotificationModule NotificationById(int id)
    {
        return _context.NotificationModule
            .First(a => a.Id == id);
    }
    
    [HttpGet("TraiterNotification/{idnotification}", Name = "TraiterNotification")]
    public IActionResult TraiterNotification(int idnotification)
    {
        NotificationModule notification = _context.NotificationModule
            .First(a => a.Id == idnotification);
        notification.Seen = true;
        _context.SaveChanges();
        return Ok($"Treated notification id "+idnotification+" at, time = "+DateTime.Now);
    }

    [HttpGet("InsertPlanningPrise/{idmodule}/{datedebut}/{datefin}/{valeurconsommation}", Name = "InsertPlanningPrise")]
    public IActionResult InsertPlanningPrise(int idmodule, DateTime datedebut, DateTime datefin, double valeurconsommation)
    {
        datedebut = datedebut.ToUniversalTime();
        datefin = datefin.ToUniversalTime();

        PlanningPrise planning = new PlanningPrise()
        {
            IdModule = idmodule,
            DateDebut = datedebut,
            DateFin = datefin,
            DateAction = DateTime.UtcNow, 
            ValeurConsommation = valeurconsommation
        };

        bool exists = _context.PlanningPrise
            .Any(p => p.IdModule == idmodule && p.DateDebut <= datefin && p.DateFin >= datedebut);

        if (!exists)
        {
            _context.Add(planning);
            _context.SaveChanges();
            return Ok($"Le planning de prise a été inséré à l'heure : {DateTime.UtcNow}");
        }
        else
        {
            return Ok("Il existe déjà un planning pour cette période.");
        }
    }

    
    [HttpGet("InsertPlanningBatterie/{idmodule}/{datedebut}/{datefin}/{valeurenergie}", Name = "InsertPlanningBatterie")]
    public IActionResult InsertPlanningBatterie(int idmodule, DateTime datedebut, DateTime datefin,
        double valeurenergie)
    {
        PlanningBatterie planning = new PlanningBatterie()
        {
            IdModule = idmodule,
            DateDebut = datedebut,
            DateFin = datefin,
            DateAction = (DateTime.Now).ToUniversalTime(),
            ValeurEnergie = valeurenergie
        };
        bool exists = _context.PlanningBatterie
            .Any(p => p.IdModule == idmodule && p.DateDebut <= datefin && p.DateFin >= datedebut);

        if (!exists)
        {
            _context.Add(planning);
            _context.SaveChanges();
            return Ok($"Le planning de prise a été inséré à l'heure : {DateTime.UtcNow}");
        }
        else
        {
            return Ok("Il existe déjà un planning pour cette période.");
        }
    }

    [HttpGet("ListePlanningPrise/{idmodule}", Name = "ListePlanningPrise")]
    public List<PlanningPrise> ListePlanningPrise(int idmodule)
    {
        return _context.PlanningPrise.Where(a => a.IdModule == idmodule)
            .ToList();
    }

    [HttpGet("ListePlanningBatterie/{idmodule}", Name = "ListePlanningBatterie")]
    public List<PlanningBatterie> ListePlanningBatterie(int idmodule)
    {
        return _context.PlanningBatterie.Where(a => a.IdModule == idmodule)
            .ToList();
    }
}