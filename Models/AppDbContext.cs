using Microsoft.EntityFrameworkCore;

namespace ImageUploaderApp.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<ImageMeta> Images => Set<ImageMeta>();
}
