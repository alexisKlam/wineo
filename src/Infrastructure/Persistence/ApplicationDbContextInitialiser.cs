using wineo.Domain.Entities;
using wineo.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection.Helpers;
using wineo.Infrastructure.Persistence.Seed;
using System.Reflection.PortableExecutable;
using System;

namespace wineo.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(RolesConst.Administrator);
        var wineExpertRole = new IdentityRole(RolesConst.WineExpert);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        if (_roleManager.Roles.All(r => r.Name != wineExpertRole.Name))
        {
            await _roleManager.CreateAsync(wineExpertRole);
        }

        //Default Users
        await CreateUserIfNotExist("WineExpert1", wineExpertRole);
        await CreateUserIfNotExist("WineExpert2", wineExpertRole);
        await CreateUserIfNotExist("Administrator", administratorRole);
        await CreateUserIfNotExist("test", wineExpertRole);
        

        if (!_context.Wines.Any())
        {
            _context.Wines.AddRange( LoadWineData());
            await _context.SaveChangesAsync();
        }

        if (!_context.WineEvaluation.Any())
        {
            _context.WineEvaluation.AddRange(await LoadWineEvaluation());
            await _context.SaveChangesAsync();
        }

        if (!_context.WinePrices.Any())
        {
            LoadWinePrices();
            await _context.SaveChangesAsync();
        }
    }

 
    private async Task CreateUserIfNotExist(string userName, IdentityRole role)
    {
        var user = new ApplicationUser {UserName = $"{userName.ToLower()}@localhost.com", Email = $"{userName.ToLower()}@localhost.com", TwoFactorEnabled = false};

        if (_userManager.Users.All(u => u.UserName != user.UserName))
        {
            await _userManager.CreateAsync(user, "Azerty_123");
            await _userManager.AddToRolesAsync(user, new[] { role.Name });
        }
    }


    private List<Wine> LoadWineData()
    {

        string csvFilePath = "InitialData/Wine_Data.csv";
        using (var reader = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.Context.RegisterClassMap<WineMap>();
            var records = csv.GetRecords<Wine>().ToList();

            return records.ToList();
           
        }
    }

    private async Task<List<WineEvaluation>> LoadWineEvaluation()
    {
        var wineEvaluations = new List<WineEvaluation>();
        string csvFilePath = "InitialData/Wine_Evaluation_Data.csv";

        var wineExperts = (await _userManager.GetUsersInRoleAsync(RolesConst.WineExpert)).ToList();

        using (var reader = new StreamReader(csvFilePath))
        {
            foreach (Wine wine in _context.Wines.ToList())
            {
                int totalEval = 0;

                while (reader.ReadLine() is { } evaluation && totalEval < 4)
                {
                   wineEvaluations.Add(new WineEvaluation
                   {
                       Appearance =Math.Round(RandomHelper.GetNextDouble() * 5,1),
                       Aroma = Math.Round(RandomHelper.GetNextDouble() * 5, 1),
                       Taste = Math.Round(RandomHelper.GetNextDouble() * 5, 1),
                       Evaluation = evaluation,
                       WineId = wine.Id,
                       AuthorId = wineExperts.GetNextRandomList().Id
                   });
                   totalEval++;
                }
            }
        }

        return wineEvaluations;

    }

    private void LoadWinePrices()
    {
        foreach (Wine wine in _context.Wines.ToList())
        {
            var wineResellers = WineReseller().GetNextXRandomItem(3);
            for (var day = 0; day < 5; day++)
            {
                _context.WinePrices.Add(new WinePrice
                {
                    WineId = wine.Id,
                    Date = DateTime.Now.AddDays(-day),
                    Price = RandomHelper.GetNextPrice(),
                    CommercialLink = wineResellers.GetNextRandomList()
                });
            }
        }
    }

    private IList<string> WineReseller() => new List<string>
    {
        "VinoGrove.com",
        "WineHarborProvisions.com",
        "CorksAndCasksEmporium.com",
        "VineyardVistaCellars.com",
        "GrapesAndGobletsMarket.com",
    };




}


