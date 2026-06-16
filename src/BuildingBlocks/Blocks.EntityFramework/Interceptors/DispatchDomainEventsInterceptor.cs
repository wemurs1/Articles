using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Blocks.EntityFrameworkCore.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator _mediator) : SaveChangesInterceptor
{
    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken ct = default)
    {
        return base.SaveChangesFailedAsync(eventData, ct);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken ct = default)
    {
        var saveResult = await base.SavedChangesAsync(eventData, result, ct);

        if (eventData.Context is not null)
            await _mediator.DispatchDomainTasksASync(eventData.Context, ct);

        return saveResult;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        return base.SavingChangesAsync(eventData, result, ct);
    }
}
