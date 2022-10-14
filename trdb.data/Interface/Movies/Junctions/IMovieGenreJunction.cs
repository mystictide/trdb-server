using trdb.entity.Movies;
using trdb.entity.Movies.Junctions;

namespace trdb.data.Interface.Movies.Junctions
{
    public interface IMovieGenreJunction : IBaseInterface<MovieGenreJunction>
    {
        Task<List<MovieGenreJunction>> Manage(List<MovieGenres> entity, int MovieID);
    }
}
