using trdb.entity.Films;
using trdb.entity.Films.Junctions;

namespace trdb.data.Interface.Films.Junctions
{
    public interface IFilmLanguageJunction : IBaseInterface<FilmLanguageJunction>
    {
        Task<List<Languages>> Manage(List<Languages> entity, int FilmID);
    }
}
