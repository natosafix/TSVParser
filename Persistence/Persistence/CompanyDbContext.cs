using Application;
using Application.Interfaces;
using Domain.EntityTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Persistence.EntityTypeConfigurations;

namespace Persistence;

public class CompanyDbContext : DbContext, ICompanyDbContext
{
    public DbSet<Department> Departments { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<JobTitle> JobTitles { get; set; }
    public DatabaseFacade Database => base.Database;

    public CompanyDbContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
        : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new JobTitleConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
