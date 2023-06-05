using Application.Commands.CreateOrUpdate.Department;
using Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;

namespace Tests;

public class CreateOrUpdateDepartmentCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task CreateOrUpdateDepartmentCommandHandler_SuccessCreate()
    {
        await CreateDepartment();
        
        Assert.NotNull(await Context.Departments.FirstOrDefaultAsync(
            department => department.Name == TestName && department.ParentId == 0));
    }
    
    [Fact]
    public async Task CreateOrUpdateDepartmentCommandHandler_SuccessUpdate()
    {
        var handler = new CreateOrUpdateDepartmentCommandHandler(Context);
        await CreateDepartment();
        
        var testPhone2 = "testPhone2";
        await handler.Handle(new CreateOrUpdateDepartmentCommand
        {
            ManagerFullName = null,
            Name = TestName,
            ParentName = "",
            Phone = testPhone2
        }, CancellationToken.None);

        Assert.True(Context.Departments.FirstOrDefaultAsync(
            department => department.Name == TestName && department.ParentId == 0).Result?.Phone == testPhone2);
    }
    
    [Fact]
    public async Task CreateOrUpdateDepartmentCommandHandler_FailOnWrongParentName()
    {
        var handler = new CreateOrUpdateDepartmentCommandHandler(Context);
        
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(
                new CreateOrUpdateDepartmentCommand
                {
                    ParentName = "fakeParent",
                    ManagerFullName = null,
                    Name = "test",
                    Phone = "testPhone"
                },
                CancellationToken.None));
    }

    [Fact]
    public async Task CreateOrUpdateDepartmentCommandHandler_SuccessOnWrongManagerFullName()
    {
        var handler = new CreateOrUpdateDepartmentCommandHandler(Context);

        await handler.Handle(
            new CreateOrUpdateDepartmentCommand
            {
                ParentName = "",
                ManagerFullName = TestFullName,
                Name = TestName,
                Phone = TestPhone
            },
            CancellationToken.None);

        Assert.NotNull(await Context.Departments.FirstOrDefaultAsync(
            department => department.Name == TestName && department.ParentId == 0));
    }
    
    [Fact]
    public async Task CreateOrUpdateDepartmentCommandHandler_SuccessHierarchy()
    {
        var handler = new CreateOrUpdateDepartmentCommandHandler(Context);
        await CreateDepartment();
        
        var testName2 = "testName2";
        var testPhone2 = "testPhone2";
        await handler.Handle(new CreateOrUpdateDepartmentCommand
        {
            ManagerFullName = null,
            Name = testName2,
            ParentName = TestName,
            Phone = testPhone2
        }, CancellationToken.None);

        var grandfather = await Context.Departments.FindAsync(0);
        var father = await Context.Departments.FirstOrDefaultAsync(
            department => department.Name == TestName && department.ParentId == 0);
        Assert.NotNull(grandfather);
        Assert.NotNull(grandfather.InverseParent.FirstOrDefault(department => department.Name == TestName));
        Assert.NotNull(father);
        Assert.True(father.InverseParent.FirstOrDefault().Name == testName2);
    }
}