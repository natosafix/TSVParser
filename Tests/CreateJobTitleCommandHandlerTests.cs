using Application.Commands.Create.JobTitle;
using Microsoft.EntityFrameworkCore;
using Tests.Common;

namespace Tests;

public class CreateJobTitleCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task CreateJobTitleCommandHandler_Success()
    {
        var handler = new CreateJobTitleCommandHandler(Context);
        var testTitle = "test";

        await handler.Handle(new CreateJobTitleCommand
        {
            Title = testTitle
        }, CancellationToken.None);
        await Context.SaveChangesAsync();
        
        Assert.NotNull(await Context.JobTitles.FirstOrDefaultAsync(jobTitle => jobTitle.Title == testTitle));
    }
    
    [Fact]
    public async Task CreateJobTitleCommandHandler_SuccessOnRecurringObject()
    {
        var handler = new CreateJobTitleCommandHandler(Context);
        var testTitle = "test";

        await handler.Handle(new CreateJobTitleCommand
        {
            Title = testTitle
        }, CancellationToken.None);
        await Context.SaveChangesAsync();
        await handler.Handle(new CreateJobTitleCommand
        {
            Title = testTitle
        }, CancellationToken.None);
        await Context.SaveChangesAsync();
        
        Assert.True(await Context.JobTitles.CountAsync() == 1);
    }
}