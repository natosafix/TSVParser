using Application.InputHandlers;
using Application.Interfaces;

namespace Application;

public class InputHandler : IInputHandler
{
    private readonly ICompanyDbContext companyDbContext;
    private readonly IFileParser fileParser;
    
    public InputHandler(ICompanyDbContext companyDbContext, IFileParser fileParser)
    {
        this.companyDbContext = companyDbContext;
        this.fileParser = fileParser;
        
    }
    public async Task Handle(string filepath, EntityInputHandler entityInputHandler)
    {
        await foreach (var parsedLines in fileParser.ParseFile(filepath))
        {
            await entityInputHandler.Handle(parsedLines);
            await companyDbContext.SaveChangesAsync(new CancellationToken());
        }
    }
}