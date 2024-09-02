using CloudSalesSystem.DBContext;
using Microsoft.EntityFrameworkCore;

namespace CloudSalesSystem.Migrations
{
    public static class MigrationEntry
    {
        public static WebApplication ApplyMigration(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CloudSalesSystemDbContext>();
            db.Database.Migrate();
            return app;
        }
    }
}
