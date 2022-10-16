using trdb.entity.Movies;

namespace trdb.data.Interface.Movies
{
    public interface ILanguages : IBaseInterface<Languages>
    {
        Task<List<Languages>> Import(List<Languages> entity);
        Task<List<Languages>> GetMovieLanguages(int MovieID);
    }
}