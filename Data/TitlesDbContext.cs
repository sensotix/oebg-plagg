using Microsoft.EntityFrameworkCore;

namespace OEBG.PLAGG.Data {
    public class TitlesDbContext(DbContextOptions<TitlesDbContext> options) : DbContext(options) {
        public DbSet<Title> Titles => Set<Title>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Title>().ToTable("Titles", "dbo");
        }
    }
}
