
namespace trdb.data.Interface.Movies
{
    public interface IMovies : IBaseInterface<entity.Movies.Movies>
    {
        Task<int> GetLatestMovie();
        Task<entity.Movies.Movies> Import(entity.Movies.Movies entity);
    }
}
