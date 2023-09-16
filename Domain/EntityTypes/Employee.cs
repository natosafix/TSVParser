namespace Domain.EntityTypes;

public class Employee
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public string FullName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int JobTitleId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual JobTitle JobTitle { get; set; } = null!;
    
    public string GetInfo(Department department, int level)
    {
        var border = new string(' ', level - 1);
        if (department.Manager == this)
            border += '*';
        else
            border += '-';
        return $"{border} {FullName} ID={Id} ({JobTitle.Title} ID={JobTitle.Id})";
    }
}
