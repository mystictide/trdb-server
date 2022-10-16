using trdb.entity.Movies;

namespace trdb.data.Interface.Movies
{
    public interface IMovieGenres : IBaseInterface<MovieGenres>
    {
        Task<List<MovieGenres>> Import(List<MovieGenres> entity);
        Task<List<MovieGenres>> GetMovieGenres(int MovieID);
    }
}
