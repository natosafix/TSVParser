using Application.Interfaces;
using Application.Queries.Get;
using Domain.EntityTypes;
using MediatR;

namespace Application;

public class OutputService
{
    private readonly IDataWriter dataWriter;
    private readonly IMediator mediator;
    private readonly IExceptionHandler exceptionHandler;

    public OutputService(IDataWriter dataWriter, IExceptionHandler exceptionHandler, IMediator mediator)
    {
        this.dataWriter = dataWriter;
        this.mediator = mediator;
        this.exceptionHandler = exceptionHandler;
    }
    
    public async Task GetDatabaseStructure()
    {
        var query = new GetDepartmentQuery {DepartmentId = 0};
        Department department;
        try
        {
            department = await mediator.Send(query);
        }
        catch (Exception e)
        {
            exceptionHandler.Handle(e);
            return;
        }

        var departmentHierarchy = new Stack<(Department department, int level)>();
        
        foreach (var child in department.InverseParent
                     .OrderByDescending(x => x.Name, StringComparer.Ordinal)
                     .Where(x => x.Id != 0))
        {
            departmentHierarchy.Push((child, 1));
            
        }

        while (departmentHierarchy.Count > 0)
        {
            var (cur, level) = departmentHierarchy.Pop();
            WriteDepartmentStructure(cur, level);
            foreach (var child in cur.InverseParent.OrderByDescending(x => x.Name, StringComparer.Ordinal))
            {
                departmentHierarchy.Push((child, level + 1));
            }
        }
    }
    
    public async Task GetDatabaseStructure(int id)
    {
        var query = new GetDepartmentQuery {DepartmentId = id};
        Department department;
        try
        {
            department = await mediator.Send(query);
        }
        catch (Exception e)
        {
            exceptionHandler.Handle(e);
            return;
        }
        
        var departmentHierarchy = new Stack<Department>();
        var cur = department;
        while (cur.Id != 0)
        {
            departmentHierarchy.Push(cur);
            cur = cur.Parent;
        }

        var level = 1;
        while (departmentHierarchy.Count != 0)
            dataWriter.WriteLine(departmentHierarchy.Pop().GetInfo(level++));
        
        WriteDepartmentStructure(department, level - 1);
    }

    private void WriteDepartmentStructure(Department cur, int level)
    {
        dataWriter.WriteLine(cur.GetInfo(level));
        if (cur.Manager is not null)
        {
            dataWriter.WriteLine(cur.Manager.GetInfo(cur, level));
        }
        foreach (var employee in cur.Employees.Where(employee => employee != cur.Manager))
        {
            dataWriter.WriteLine(employee.GetInfo(cur, level));
        }
    }
}