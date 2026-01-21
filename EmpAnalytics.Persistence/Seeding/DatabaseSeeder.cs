using EmpAnalytics.Domain.Jobs;
using EmpAnalytics.Domain.UserJobs;
using EmpAnalytics.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EmpAnalytics.Persistence.Seeding;

public static class DatabaseSeeder
{
    private static readonly string[] FirstNames =
    [
        "Jonas", "Petras", "Antanas", "Mindaugas", "Tomas", "Lukas", "Matas", "Domas", "Kajus", "Rokas",
        "Gabrielė", "Emilija", "Austėja", "Ugnė", "Greta", "Ieva", "Kamilė", "Eglė", "Simona", "Viktorija",
        "Paulius", "Arnas", "Deividas", "Justinas", "Karolis", "Vytautas", "Ignas", "Augustas", "Nojus", "Dominykas",
        "Monika", "Gintarė", "Rūta", "Aistė", "Neringa", "Indrė", "Laura", "Patricija", "Gabija", "Karina",
        "Martynas", "Edvinas", "Tautvydas", "Šarūnas", "Evaldas", "Aurimas", "Rimantas", "Algirdas", "Saulius",
        "Kęstutis",
        "Kristina", "Vaida", "Jolanta", "Daiva", "Vilma", "Lina", "Raminta", "Sandra", "Inga", "Aušra"
    ];

    private static readonly string[] LastNames =
    [
        "Kazlauskas", "Jankauskas", "Petrauskas", "Stankevičius", "Vasiliauskas", "Žukauskas", "Butkus",
        "Paulauskas", "Urbonas", "Kavaliauskas", "Sakalauskas", "Rimkus", "Kairys", "Balčiūnas", "Čeponis",
        "Navickas", "Mockus", "Pocius", "Brazaitis", "Mažeika", "Mickus", "Šimkus", "Baranauskas",
        "Grigas", "Norkus", "Gedminas", "Zabiela", "Vaičiūnas", "Milčius", "Juodaitis",
        "Valančius", "Petkevičius", "Rutkauskas", "Sabaliauskas", "Kudirka", "Žemaitis", "Dapkus",
        "Kriščiūnas", "Žilinskas", "Bielskis", "Montvila", "Norvilas", "Steponavičius", "Jasaitis", "Bakaitis"
    ];

    private static readonly string[] JobNames =
    [
        "Sukūrė programinę įrangą",
        "Atliko testavimą",
        "Vadovavo projektui",
        "Administravo sistemas",
        "Atliko duomenų analizę",
        "Įdiegė DevOps sprendimus",
        "Sukūrė vartotojo sąsają",
        "Užtikrino kokybę",
        "Suprojektavo techninę architektūrą",
        "Moderavo Scrum procesus",
        "Įgyvendino naują funkcionalumą",
        "Optimizavo sistemos našumą",
        "Parengė techninę dokumentaciją",
        "Atliko kodo peržiūrą",
        "Integravo išorines sistemas",
        "Automatizavo testavimo procesus",
        "Išsprendė kritines klaidas",
        "Diegė sistemą gamybinėje aplinkoje",
        "Konfigūravo serverius",
        "Atliko saugumo patikrą",
        "Analizavo verslo reikalavimus",
        "Sukūrė duomenų modelį",
        "Migravo duomenis",
        "Prižiūrėjo sistemos veikimą",
        "Atnaujino programinę įrangą",
        "Parengė diegimo planą",
        "Įdiegė CI/CD procesą",
        "Testavo sistemos apkrovą",
        "Koordinavo IT darbus",
        "Teikė techninę pagalbą"
    ];


    public static async Task SeedAsync(ApplicationDbContext context, bool useLargeSeed = true)
    {
        if (await context.Jobs.AnyAsync())
            return;

        var random = Random.Shared;

        context.ChangeTracker.AutoDetectChangesEnabled = false;

        var jobs = JobNames.Select(name => new Job(Guid.NewGuid(), name)).ToList();
        await context.Jobs.AddRangeAsync(jobs);
        await context.SaveChangesAsync();

        var userCount = useLargeSeed ? 10_000 : 100;
        var users = new List<User>(userCount);
        for (var i = 0; i < userCount; i++)
        {
            var firstName = FirstNames[random.Next(FirstNames.Length)];
            var lastName = LastNames[random.Next(LastNames.Length)];
            users.Add(new User(Guid.NewGuid(), firstName, lastName));
        }

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        var userJobCount = useLargeSeed ? 1_000_000 : 10_000;
        var batchSize = 50_000;
        var endDate = DateTime.UtcNow;
        var startDate = endDate.AddMonths(-12);
        var tickRange = (endDate - startDate).Ticks;

        for (var batch = 0; batch < userJobCount; batch += batchSize)
        {
            var currentBatchSize = Math.Min(batchSize, userJobCount - batch);
            var userJobs = new List<UserJob>(currentBatchSize);

            for (var i = 0; i < currentBatchSize; i++)
            {
                var user = users[random.Next(users.Count)];
                var job = jobs[random.Next(jobs.Count)];
                var dateCreated = startDate.AddTicks(random.NextInt64(0, tickRange));

                userJobs.Add(new UserJob(user.UserId, job.JobId, dateCreated));
            }

            await context.UserJobs.AddRangeAsync(userJobs);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
        }

        context.ChangeTracker.AutoDetectChangesEnabled = true;
    }
}