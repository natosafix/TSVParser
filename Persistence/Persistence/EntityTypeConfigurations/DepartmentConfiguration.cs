using Domain.EntityTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(e => e.Id).HasName("departments_pkey");

        builder.ToTable("departments");
        
        builder.HasIndex(e => new { e.ParentId, e.Name }, "departments_parent_id_name_key").IsUnique();

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ManagerId).HasColumnName("manager_id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.ParentId).HasColumnName("parent_id");
        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .HasColumnName("phone");

        builder.HasOne(d => d.Manager).WithMany(p => p.Departments)
            .HasForeignKey(d => d.ManagerId)
            .HasConstraintName("departments_manager_id_fkey");

        builder.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("departments_parent_id_fkey");
    }
}