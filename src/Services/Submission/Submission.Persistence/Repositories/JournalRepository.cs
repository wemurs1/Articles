using Submission.Domain.Entities;

namespace Submission.Persistence.Repositories;

public class JournalRepository(SubmissionDbContext context) : Repository<Journal>(context)
{

}
