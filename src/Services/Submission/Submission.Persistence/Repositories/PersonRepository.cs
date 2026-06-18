namespace Submission.Persistence.Repositories;

public class PersonRepository(SubmissionDbContext context, CancellationToken ct = default)
{
    private readonly SubmissionDbContext _context = context;
    private readonly CancellationToken _ct = ct;

    public async Task<Person?> GetByUserIdAsync(int userId)
        => await _context.Set<Person>().SingleOrDefaultAsync(x => x.Id == userId, _ct);

    public async Task AddAsync(Person person, CancellationToken ct = default)
    {
        await _context.Set<Person>().AddAsync(person);
    }
}
