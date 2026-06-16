using Microsoft.EntityFrameworkCore;
using Auth.Domain.Persons;
using Blocks.EntityFrameworkCore;

namespace Auth.Persistence.Repositories;

public class PersonRepository(AuthDbContext dbContext) : Repository<AuthDbContext, Person>(dbContext)
{
    public async Task<Person?> GetByUserIdAsync(int userId, CancellationToken ct = default)
        => await Query().SingleOrDefaultAsync(e => e.UserId == userId, ct);

    public async Task<Person?> GetByEmailASync(string email, CancellationToken ct = default)
        => await Query().SingleOrDefaultAsync(e => e.Email.NormalisedEmail == email.ToUpperInvariant(), ct);

    protected override IQueryable<Person> Query() => base.Query().Include(p => p.User);
}

