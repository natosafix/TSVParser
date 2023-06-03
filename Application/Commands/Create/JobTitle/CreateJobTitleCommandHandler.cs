using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Create.JobTitle;

public class CreateJobTitleCommandHandler : IRequestHandler<CreateJobTitleCommand>
{
    private readonly ICompanyDbContext dbContext;
    
    public CreateJobTitleCommandHandler(ICompanyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<Unit> Handle(CreateJobTitleCommand request, CancellationToken cancellationToken)
    {
        
        if (await dbContext.JobTitles.FirstOrDefaultAsync(e => e.Title == request.Title, cancellationToken) != null)
            return Unit.Value;

        await dbContext.JobTitles.AddAsync(new Domain.EntityTypes.JobTitle
        {
            Title = request.Title
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}