using trdb.entity.Films;
using trdb.entity.Films.Junctions;

namespace trdb.data.Interface.Films.Junctions
{
    public interface IFilmCreditsJunction : IBaseInterface<FilmCreditsJunction>
    {
        Task<List<People>> Manage(List<People> entity, int FilmID);
    }
}
