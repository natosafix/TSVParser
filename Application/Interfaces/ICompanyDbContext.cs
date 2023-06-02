using Domain.EntityTypes;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface ICompanyDbContext
{
    DbSet<Department> Departments { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<JobTitle> JobTitles { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}