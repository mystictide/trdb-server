using trdb.entity.Films;

namespace trdb.data.Interface.Films
{
    public interface ILanguages : IBaseInterface<Languages>
    {
        Task<List<Languages>> Import(List<Languages> entity);
        Task<List<Languages>> GetFilmLanguages(int FilmID);
    }
}