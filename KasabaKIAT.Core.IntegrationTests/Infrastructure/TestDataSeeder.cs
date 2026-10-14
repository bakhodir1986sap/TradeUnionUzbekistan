using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;

namespace KasabaKIAT.Core.IntegrationTests.Infrastructure;

public class SeedData
{
    public long AdminRoleId { get; set; }
    public long UserRoleId { get; set; }
    public long AdminUserId { get; set; }
    public long RegularUserId { get; set; }
    public long SanatoriumId { get; set; }
    public long BranchId { get; set; }
}

public static class TestDataSeeder
{
    public static async Task<SeedData> SeedAsync(KasabaKIATContext db)
    {
        var seed = new SeedData();

        // Roles
        var adminRole = new Role { name = "admin" };
        var userRole = new Role { name = "user" };
        db.Roles.AddRange(adminRole, userRole);
        await db.SaveChangesAsync();
        seed.AdminRoleId = adminRole.id;
        seed.UserRoleId = userRole.id;

        // Users — plain-text password so uspLogin test proc can authenticate
        var adminUser = new User
        {
            user_name = "testadmin",
            password = "Admin123!",
            email = "testadmin@test.com",
            role_id = adminRole.id,
            state = "1",
            account_locked = false,
            account_expired = false,
            bkut_id = 1
        };
        var regularUser = new User
        {
            user_name = "testuser",
            password = "User123!",
            email = "testuser@test.com",
            role_id = userRole.id,
            state = "1",
            account_locked = false,
            account_expired = false,
            bkut_id = 1
        };
        db.Users.AddRange(adminUser, regularUser);
        await db.SaveChangesAsync();
        seed.AdminUserId = adminUser.id;
        seed.RegularUserId = regularUser.id;

        // Sanatorium reference (FK required by putyovki)
        var san = new TblSpravSan { Name_San = "Test Sanatorium", Num_Kod = "001" };
        db.TblSpravSans.Add(san);
        await db.SaveChangesAsync();
        seed.SanatoriumId = san.ID;

        // Branch reference (used by ReferenceController/SpBranchController tests)
        var branch = new SpBranch { name_ru = "Test Branch", name_uz = "Test Branch", name_uzl = "Test Branch" };
        db.SpBranches.Add(branch);
        await db.SaveChangesAsync();
        seed.BranchId = branch.id;

        return seed;
    }
}
