using trdb.entity.Movies;
using trdb.entity.Movies.Junctions;

namespace trdb.data.Interface.Movies.Junctions
{
    public interface IMovieCreditsJunction : IBaseInterface<MovieCreditsJunction>
    {
        Task<List<People>> Manage(List<People> entity, int MovieID);
    }
}
