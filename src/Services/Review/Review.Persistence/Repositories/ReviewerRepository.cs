namespace Review.Persistence.Repositories;

public class ReviewerRepository(ReviewDbContext dbContext) : Repository<Reviewer>(dbContext)
{
    public async Task<Reviewer?> GetByUserIdAsync(int userId, CancellationToken ct = default)
        => await Query().SingleOrDefaultAsync(r => r.UserId == userId, ct);
}
