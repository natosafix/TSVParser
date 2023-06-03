using Domain.EntityTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Interfaces;

public interface ICompanyDbContext
{
    DbSet<Department> Departments { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<JobTitle> JobTitles { get; set; }
    DatabaseFacade Database { get;}
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}