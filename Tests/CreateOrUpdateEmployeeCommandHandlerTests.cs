using Application.Commands.CreateOrUpdate.Employee;
using Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;

namespace Tests;

public class CreateOrUpdateEmployeeCommandHandlerTests : TestCommandBase
{   
    [Fact]
    public async Task CreateOrUpdateDepartmentCommandHandler_SuccessCreate()
    {
        
        await CreateDepartment();
        await CreateJobTitle();
        await CreateEmployee();
        var employee = await Context.Employees.FirstOrDefaultAsync(employee => employee.FullName == TestFullName);
        Assert.NotNull(employee);
        Assert.True(employee?.Department.Name == TestName && employee.JobTitle.Title == TestTitle &&
                    employee.Login == TestLogin && employee.Password == TestPassword);
    }
    
    [Fact]
    public async Task CreateOrUpdateDepartmentCommandHandler_SuccessUpdate()
    {
        
        await CreateDepartment();
        await CreateJobTitle();
        await CreateEmployee();
        const string testLogin2 = "test2";
        const string testPassword2 = "test2";
        var handler = new CreateOrUpdateEmployeeCommandHandler(Context);
        await handler.Handle(new CreateOrUpdateEmployeeCommand
        {
            DepartmentName = TestName,
            FullName = TestFullName,
            Login = testLogin2,
            Password = testPassword2,
            JobTitle = TestTitle,
        }, CancellationToken.None);
        await Context.SaveChangesAsync();
        var employee = await Context.Employees.FirstOrDefaultAsync(employee => employee.FullName == TestFullName);
        Assert.NotNull(employee);
        Assert.True(employee?.Department.Name == TestName && employee.JobTitle.Title == TestTitle &&
                    employee.Login == testLogin2 && employee.Password == testPassword2);
    }
    
    [Fact]
    public async Task CreateOrUpdateDepartmentCommandHandler_FailOnWrongJobTitle()
    {
        
        await CreateDepartment();
        await CreateJobTitle();
        var handler = new CreateOrUpdateEmployeeCommandHandler(Context);
        await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(new CreateOrUpdateEmployeeCommand
        {
            DepartmentName = TestName,
            FullName = TestFullName,
            Login = TestLogin,
            Password = TestPassword,
            JobTitle = "fakeJob"
        }, CancellationToken.None));
    }
    
    [Fact]
    public async Task CreateOrUpdateDepartmentCommandHandler_FailOnWrongDepartment()
    {
        
        await CreateDepartment();
        await CreateJobTitle();
        var handler = new CreateOrUpdateEmployeeCommandHandler(Context);
        await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(new CreateOrUpdateEmployeeCommand
        {
            DepartmentName = "fakeDepartment",
            FullName = TestFullName,
            Login = TestLogin,
            Password = TestPassword,
            JobTitle = TestTitle
        }, CancellationToken.None));
    }
}