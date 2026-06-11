using Microsoft.EntityFrameworkCore;
using Submission.Domain.Entities;

namespace Submission.Persistence.Repositories;

public class PersonRepository(SubmissionDbContext context, CancellationToken ct = default) : Repository<Person>(context)
{
    public async Task<Person?> GetByUserIdAsync(int userId) => await _entity.SingleOrDefaultAsync(x => x.Id == userId, ct);
}
