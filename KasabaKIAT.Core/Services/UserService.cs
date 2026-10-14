using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Users;
using KasabaKIAT.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Services;

public class UserService(KasabaKIATContext db, IAuditService audit, ILogger<UserService> logger) : IUserService
{
    public async Task<(List<UserDto> Items, int Total)> GetUsersAsync(int page, int pageSize, string? search)
    {
        var query = db.Users.Include(u => u.role).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u => (u.user_name != null && u.user_name.Contains(search)) ||
                                     (u.email != null && u.email.Contains(search)));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(u => u.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => ToDto(u))
            .ToListAsync();

        return (items, total);
    }

    public async Task<UserDto?> GetByIdAsync(long id)
    {
        var user = await db.Users.Include(u => u.role).FirstOrDefaultAsync(u => u.id == id);
        return user == null ? null : ToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request)
    {
        var user = new User
        {
            user_name = request.Login,
            password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            email = request.Email,
            role_id = request.RoleId,
            bkut_id = request.BkutId,
            dep_id = request.DepId,
            REGION_ID = request.RegionId,
            BRANCH_ID = request.BranchId,
            language = request.Language,
            usertype = request.UserType,
            account_locked = false,
            account_expired = false,
            state = "1",
            version = 0
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        await db.Entry(user).Reference(u => u.role).LoadAsync();
        logger.LogInformation("User created: Id={UserId} UserName={UserName}", user.id, request.Login);
        await audit.LogAsync(user.id, "USER_CREATE", $"UserName={request.Login}");
        return ToDto(user);
    }

    public async Task<UserDto?> UpdateAsync(long id, UpdateUserRequest request)
    {
        var user = await db.Users.Include(u => u.role).FirstOrDefaultAsync(u => u.id == id);
        if (user == null) return null;

        if (request.Email != null) user.email = request.Email;
        if (request.RoleId.HasValue) user.role_id = request.RoleId;
        if (request.DepId.HasValue) user.dep_id = request.DepId;
        if (request.RegionId != null) user.REGION_ID = request.RegionId;
        if (request.BranchId.HasValue) user.BRANCH_ID = request.BranchId.Value;
        if (request.IsSuperAdmin.HasValue) user.is_super_admin = request.IsSuperAdmin;
        if (request.AccountLocked.HasValue) user.account_locked = request.AccountLocked.Value;
        if (request.AccountExpired.HasValue) user.account_expired = request.AccountExpired.Value;
        if (request.State != null) user.state = request.State;
        if (request.Language != null) user.language = request.Language;

        await db.SaveChangesAsync();
        await db.Entry(user).Reference(u => u.role).LoadAsync();
        logger.LogInformation("User updated: Id={UserId}", id);
        await audit.LogAsync(id, "USER_UPDATE", $"TargetUserId={id}");
        return ToDto(user);
    }

    public async Task<bool> DeactivateAsync(long id)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null) return false;
        user.state = "0";
        await db.SaveChangesAsync();
        logger.LogInformation("User deactivated: Id={UserId}", id);
        await audit.LogAsync(id, "USER_DEACTIVATE", $"TargetUserId={id}");
        return true;
    }

    private static UserDto ToDto(User u) => new()
    {
        Id = u.id,
        Email = u.email,
        UserName = u.user_name,
        RoleId = u.role_id,
        RoleName = u.role?.name,
        BkutId = u.bkut_id,
        DepId = u.dep_id,
        RegionId = u.REGION_ID,
        BranchId = u.BRANCH_ID,
        IsSuperAdmin = u.is_super_admin,
        AccountLocked = u.account_locked,
        AccountExpired = u.account_expired,
        State = u.state,
        Language = u.language,
        UserType = u.usertype
    };
}
