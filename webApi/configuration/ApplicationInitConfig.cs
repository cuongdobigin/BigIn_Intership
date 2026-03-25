

using Microsoft.EntityFrameworkCore;
using webApi.Infrastructure.Persistence;
using Myapp.Domain.Entity;

namespace webApi.configuration;

public class ApplicationInitConfig
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Migrate
        await db.Database.MigrateAsync();

        // Seed nếu chưa có admin
        if (!await db.Accounts.AnyAsync(a => a.username == "admin"))
        {
            // Seed Roles nếu chưa có
            var adminRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "admin");
            if (adminRole == null)
            {
                adminRole = new Role { Name = "admin" };
                db.Roles.Add(adminRole);

                db.Roles.Add(new Role { Name = "user" });
                await db.SaveChangesAsync();
            }

            // Seed Account admin
            var account = new Account
            {
                username  = "admin",
                password  = BCrypt.Net.BCrypt.HashPassword("admin123"),
                isActive  = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Roles     = new List<Role> {adminRole }
            };

            db.Accounts.Add(account);
            await db.SaveChangesAsync();
        }
    }
}