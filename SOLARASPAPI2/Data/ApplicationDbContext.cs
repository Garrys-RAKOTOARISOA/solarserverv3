using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ASPAPI.Models;

namespace ASPAPI.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<ASPAPI.Models.Client> Client { get; set; } = default!;
    
    public DbSet<ASPAPI.Models.Batterie> Batterie { get; set; } = default!;
    
    public DbSet<ASPAPI.Models.ModuleSolar> Module { get; set; } = default!;
    
    public DbSet<ASPAPI.Models.BatterieData> BatterieData { get; set; } = default!;
    
    public DbSet<ASPAPI.Models.PriseData> PriseData { get; set; } = default!;
    
    public DbSet<ASPAPI.Models.PanneauData> PanneauData { get; set; } = default!;
    public DbSet<ASPAPI.Models.DureeUtilisationBatterie> DureeUtilisationBatterie { get; set; } = default!;
    public DbSet<ASPAPI.Models.TotalConsommationPrise> TotalConsommationPrise { get; set; } = default!;
    
    public DbSet<ASPAPI.Models.TotalProductionPanneau> TotalProductionPanneau { get; set; } = default!;
    public DbSet<ASPAPI.Models.NotificationModule> NotificationModule { get; set; } = default!;
    public DbSet<ASPAPI.Models.CouleurBoutonBatterie> CouleurBoutonBatterie { get; set; } = default!;
    public DbSet<ASPAPI.Models.CouleurBoutonPrise> CouleurBoutonPrise { get; set; } = default!;
    
    public DbSet<ASPAPI.Models.PlanningBatterie> PlanningBatterie { get; set; } = default!;
    public DbSet<ASPAPI.Models.PlanningPrise> PlanningPrise { get; set; } = default!;
    
    public DbSet<ASPAPI.Models.RelaisPrise> RelaisPrise { get; set; } = default!;
    public DbSet<ASPAPI.Models.RelaisBatterie> RelaisBatterie { get; set; } = default!;
}