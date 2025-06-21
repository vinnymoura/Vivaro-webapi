using Application.Shared.Context;
using Application.Shared.Services.Abstractions;

namespace Application.Shared.Services;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
        await dbContext.SaveChangesAsync(cancellationToken);
}