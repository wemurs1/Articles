namespace Review.Persistence.Repositories;

public class ArticleRepository(ReviewDbContext dbContext) : Repository<Article>(dbContext)
{
    public Task<Article> GetFullArticleById(int id)
    {
        throw new NotImplementedException();
    }
}
