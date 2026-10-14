using KasabaKIAT.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Data;

public partial class KasabaKIATContext(DbContextOptions<KasabaKIATContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public virtual DbSet<Functionality> Functionalities { get; set; } = null!;
    public virtual DbSet<TbptPutyovki> TbptPutyovkis { get; set; } = null!;
    public virtual DbSet<TblSpravSan> TblSpravSans { get; set; } = null!;
    public virtual DbSet<SpSanatoriumType> SpSanatoriumTypes { get; set; } = null!;
    public virtual DbSet<TblSpravReg> TblSpravRegs { get; set; } = null!;
    public virtual DbSet<SpBranch> SpBranches { get; set; } = null!;
    public virtual DbSet<SpCountry> SpCountries { get; set; } = null!;
    public virtual DbSet<SpPosition> SpPositions { get; set; } = null!;
    public virtual DbSet<TDepartmentBase> TDepartmentBases { get; set; } = null!;
    public virtual DbSet<ApiRefreshToken> ApiRefreshTokens { get; set; } = null!;
    public virtual DbSet<TCamp> TCamps { get; set; } = null!;
    public virtual DbSet<TbptZaezdi> TbptZaezdis { get; set; } = null!;
    public virtual DbSet<TblSpravKv> TblSpravKvs { get; set; } = null!;
    public virtual DbSet<TbcsDictionary> TbcsDictionaries { get; set; } = null!;
    public virtual DbSet<TbcsDictionaryType> TbcsDictionaryTypes { get; set; } = null!;
    public virtual DbSet<SpSoato> SpSoatos { get; set; } = null!;
    public virtual DbSet<SpNation> SpNations { get; set; } = null!;
    public virtual DbSet<SpDegree> SpDegrees { get; set; } = null!;
    public virtual DbSet<SpContractType> SpContractTypes { get; set; } = null!;
    public virtual DbSet<SpDepartmentType> SpDepartmentTypes { get; set; } = null!;
    public virtual DbSet<SpSection> SpSections { get; set; } = null!;
    public virtual DbSet<SpSanatoriumProfil> SpSanatoriumProfils { get; set; } = null!;
    public virtual DbSet<SpOkonx> SpOkonxes { get; set; } = null!;
    public virtual DbSet<SpOpf> SpOpfs { get; set; } = null!;
    public virtual DbSet<SpEmployees> SpEmployees { get; set; } = null!;
    public virtual DbSet<TblSpravOtras> TblSpravOtrases { get; set; } = null!;
    public virtual DbSet<TblSpravVil> TblSpravVils { get; set; } = null!;
    public virtual DbSet<TblSpravMfo> TblSpravMfos { get; set; } = null!;
    public virtual DbSet<TblSpravFace> TblSpravFaces { get; set; } = null!;
    public virtual DbSet<TblSpravOs> TblSpravOses { get; set; } = null!;
    public virtual DbSet<TblSpravPr> TblSpravPrs { get; set; } = null!;
    public virtual DbSet<TNormativDoc> TNormativDocs { get; set; } = null!;
    public virtual DbSet<TSanatorium> TSanatoriums { get; set; } = null!;
    public virtual DbSet<UserAction> UserActions { get; set; } = null!;
    public virtual DbSet<SpSoogu> SpSoogus { get; set; } = null!;
    public virtual DbSet<TblSpravOrg> TblSpravOrgs { get; set; } = null!;
    public virtual DbSet<TbptYearlyPlanRegion> TbptYearlyPlanRegions { get; set; } = null!;
    public virtual DbSet<TbptYearlyPlanOtrasl> TbptYearlyPlanOtrasls { get; set; } = null!;
    public virtual DbSet<TbptAvaragePrice> TbptAvaragePrices { get; set; } = null!;
    public virtual DbSet<TblSpisokRaspredeleniya> TblSpisokRaspredeleniyas { get; set; } = null!;
    public virtual DbSet<TbkuGrafikZaezdov> TbkuGrafikZaezdovs { get; set; } = null!;
    public virtual DbSet<TbkuStrokaProfsoyza> TbkuStrokaProfsoyazs { get; set; } = null!;
    public virtual DbSet<TEgrpo> TEgrpos { get; set; } = null!;
    public virtual DbSet<TblBron> TblBrons { get; set; } = null!;
    public virtual DbSet<TbbuFinOtchetSetting> TbbuFinOtchetSettings { get; set; } = null!;
    public virtual DbSet<TbbuPlatejka> TbbuPlatejkas { get; set; } = null!;
    public virtual DbSet<TblPosPut> TblPosPuts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.id);
            entity.HasOne(e => e.role)
                  .WithMany(r => r.users)
                  .HasForeignKey(e => e.role_id)
                  .IsRequired(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e => e.id);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("role_permission");
            entity.HasKey(e => e.id);
            entity.HasOne(e => e.role)
                  .WithMany(r => r.role_permissions)
                  .HasForeignKey(e => e.role_id)
                  .IsRequired(false);
            entity.HasOne(e => e.functionality)
                  .WithMany(f => f.role_permissions)
                  .HasForeignKey(e => e.functionality_id)
                  .IsRequired(false);
        });

        modelBuilder.Entity<TbptPutyovki>(entity =>
        {
            entity.ToTable("tbpt_putyovki");
            entity.HasKey(e => e.REC_ID);
            entity.HasOne(e => e.tblSprav_San)
                  .WithMany(s => s.tbpt_putyovkis)
                  .HasForeignKey(e => e.SANATORIUM_ID)
                  .IsRequired(false);
            entity.HasOne(e => e.user)
                  .WithMany()
                  .HasForeignKey(e => e.USER_ID)
                  .IsRequired(false);
        });

        modelBuilder.Entity<TblSpravSan>(entity =>
        {
            entity.ToTable("tblSprav_San");
            entity.HasKey(e => e.ID);
        });

        modelBuilder.Entity<ApiRefreshToken>(entity =>
        {
            entity.ToTable("api_refresh_tokens");
            entity.HasKey(e => e.id);
            entity.Property(e => e.created_at).HasDefaultValueSql("GETDATE()");
        });

        base.OnModelCreating(modelBuilder);
    }
}
