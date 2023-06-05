using System.Data;
using Application.Commands.Create.JobTitle;
using Application.Commands.CreateOrUpdate.Department;
using Application.Commands.CreateOrUpdate.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;

namespace Tests.Common;

public abstract class TestCommandBase : IDisposable
{
    protected const string TestName = "testName";
    protected const string TestPhone = "testPhone";
    protected const string TestFullName = "Тестов Тест Тестович";
    protected const string TestLogin = "test";
    protected const string TestPassword = "test";
    protected const string TestTitle = "test";
    
    protected readonly CompanyDbContext Context;

    public TestCommandBase()
    {
        Context = CompanyContextFactory.Create();
    }

    public void Dispose()
    {
        CompanyContextFactory.Destroy(Context);
    }
    
    protected async Task CreateDepartment()
    {
        var handler = new CreateOrUpdateDepartmentCommandHandler(Context);
        await handler.Handle(new CreateOrUpdateDepartmentCommand
        {
            ManagerFullName = null,
            Name = TestName,
            ParentName = "",
            Phone = TestPhone
        }, CancellationToken.None);
    }
    
    protected async Task CreateJobTitle()
    {
        var handler = new CreateJobTitleCommandHandler(Context);
        await handler.Handle(new CreateJobTitleCommand
        {
            Title = TestTitle
        }, CancellationToken.None);
        await Context.SaveChangesAsync();
    }
    
    protected async Task CreateEmployee()
    {
        var handler = new CreateOrUpdateEmployeeCommandHandler(Context);
        await handler.Handle(new CreateOrUpdateEmployeeCommand
        {
            DepartmentName = TestName,
            FullName = TestFullName,
            Login = TestLogin,
            Password = TestPassword,
            JobTitle = TestTitle,
        }, CancellationToken.None);

        await Context.SaveChangesAsync();
    }
}
