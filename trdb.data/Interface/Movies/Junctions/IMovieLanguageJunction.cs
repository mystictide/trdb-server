using trdb.entity.Movies;
using trdb.entity.Movies.Junctions;

namespace trdb.data.Interface.Movies.Junctions
{
    public interface IMovieLanguageJunction : IBaseInterface<MovieLanguageJunction>
    {
        Task<List<MovieLanguageJunction>> Manage(List<Languages> entity, int MovieID);
    }
}
