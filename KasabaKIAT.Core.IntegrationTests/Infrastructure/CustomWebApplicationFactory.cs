using KasabaKIAT.Core.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KasabaKIAT.Core.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public static readonly string TestConnectionString =
        Environment.GetEnvironmentVariable("TEST_DB_CONNECTION")
        ?? "Server=W-PF36JE4W\\SQLEXPRESS;Database=KasabaKIAT_Test;User Id=sa;Password=123;TrustServerCertificate=True;";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");
        builder.UseSetting("ConnectionStrings:DefaultConnection", TestConnectionString);

        builder.ConfigureServices(services =>
        {
            // Replace the DbContext registration with the test connection string
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<KasabaKIATContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<KasabaKIATContext>(options =>
                options.UseSqlServer(TestConnectionString));
        });
    }
}
