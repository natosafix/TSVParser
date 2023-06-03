using Application.Interfaces;
using Application.Queries.Get;
using Domain.EntityTypes;
using MediatR;

namespace Application;

public class OutputScenario
{
    private readonly IDataWriter dataWriter;
    private readonly IMediator mediator;
    private readonly IExceptionHandler exceptionHandler;

    public OutputScenario(IDataWriter dataWriter, IMediator mediator, IExceptionHandler exceptionHandler)
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
        
        foreach (var child in department.InverseParent.Where(x => x.Id != 0))
        {
            WriteDepartmentStructure(child, 1);
        }
    }

    private void WriteDepartmentStructure(Department cur, int level)
    {
        dataWriter.WriteLine(cur.GetInfo(level));
        if (cur.Manager is not null)
        {
            dataWriter.WriteLine(cur.Manager.GetInfo(cur, level));
        }
        foreach (var employee in cur.Employees)
        {
            if (employee != cur.Manager)
                dataWriter.WriteLine(employee.GetInfo(cur, level));
        }

        foreach (var child in cur.InverseParent.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            WriteDepartmentStructure(child, level + 1);
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
        {
            dataWriter.WriteLine(departmentHierarchy.Pop().GetInfo(level++));
        }

        foreach (var employee in department.Employees)
        {
            dataWriter.WriteLine(employee.GetInfo(department, level - 1));
        }
    }
}