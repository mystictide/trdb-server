using trdb.entity.Films;
using trdb.entity.Films.Junctions;

namespace trdb.data.Interface.Films.Junctions
{
    public interface IFilmGenreJunction : IBaseInterface<FilmGenreJunction>
    {
        Task<List<FilmGenres>> Manage(List<FilmGenres> entity, int FilmID);
    }
}
