using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application;

public interface ICompanyDbContext
{
    DbSet<Department> Departments { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<JobTitle> JobTitles { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}