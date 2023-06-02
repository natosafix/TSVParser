using Domain.EntityTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id).HasName("employees_pkey");

        builder.ToTable("employees");

        builder.HasIndex(e => e.FullName, "employees_full_name_key").IsUnique();

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.DepartmentId).HasColumnName("department_id");
        builder.Property(e => e.FullName).HasColumnName("full_name");
        builder.Property(e => e.JobTitleId).HasColumnName("job_title_id");
        builder.Property(e => e.Login)
            .HasMaxLength(25)
            .HasColumnName("login");
        builder.Property(e => e.Password)
            .HasMaxLength(50)
            .HasColumnName("password");

        builder.HasOne(d => d.Department).WithMany(p => p.Employees)
            .HasForeignKey(d => d.DepartmentId)
            .HasConstraintName("fk_employees_departments");

        builder.HasOne(d => d.JobTitle).WithMany(p => p.Employees)
            .HasForeignKey(d => d.JobTitleId)
            .HasConstraintName("employees_job_title_id_fkey");
    }
}