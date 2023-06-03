﻿namespace Domain.EntityTypes;

public class Department
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public int? ManagerId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Department> InverseParent { get; set; } = new List<Department>();

    public virtual Employee? Manager { get; set; }

    public virtual Department Parent { get; set; } = null!;

    public string GetInfo(int level)
    {
        var border = new string('=', level);
        return $"{border} {Name} ID={Id}";
    }
}
