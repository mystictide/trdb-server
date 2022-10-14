
namespace trdb.data.Interface.Movies
{
    public interface IMovies : IBaseInterface<entity.Movies.Movies>
    {
        Task<entity.Movies.Movies> Import(entity.Movies.Movies entity);
    }
}
