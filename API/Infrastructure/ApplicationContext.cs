using Microsoft.EntityFrameworkCore;
using WebApplication1;

public class ApplicationContext : DbContext
{
    public DbSet<Prompt> Prompts { get; set; } = null!;
 
    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=PromptBase;Username=postgres;Password=ifconfigroute-n");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prompt>().Property(p => p.NameOfPrompt).IsRequired();
        modelBuilder.Entity<Prompt>().Property(p => p.Description).IsRequired();
    }
}