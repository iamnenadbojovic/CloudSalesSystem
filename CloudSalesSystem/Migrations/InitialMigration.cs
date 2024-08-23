using Microsoft.EntityFrameworkCore;

namespace CloudSalesSystem.Migrations
{
    public static class MigrationEntry
    {
        public static void ApplyMigration<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
        {
            using TDbContext context = scope.ServiceProvider
                .GetRequiredService<TDbContext>();

            context.Database.Migrate();
        }
    }
}
