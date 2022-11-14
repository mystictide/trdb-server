using trdb.entity.Films;

namespace trdb.data.Interface.Films
{
    public interface IFilmGenres : IBaseInterface<FilmGenres>
    {
        Task<List<FilmGenres>> Import(List<FilmGenres> entity);
        Task<List<FilmGenres>> GetFilmGenres(int FilmID);
    }
}
