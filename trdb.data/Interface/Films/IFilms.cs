
namespace trdb.data.Interface.Films
{
    public interface IFilms : IBaseInterface<entity.Films.Films>
    {
        Task<int> GetLatestFilm();
        Task<entity.Films.Films> GetFilmDetails(int? ID, string? title);
        Task<entity.Films.Films> Import(entity.Films.Films entity);
    }
}
