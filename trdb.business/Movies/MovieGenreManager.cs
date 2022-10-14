using trdb.data.Interface.Movies;
using trdb.data.Repo.MovieGenres;
using trdb.entity.Helpers;
using trdb.entity.Movies;

namespace trdb.business.Movies
{
    public class MovieGenreManager : IMovieGenres
    {
        private readonly IMovieGenres _repo;
        public MovieGenreManager()
        {
            _repo = new MovieGenresRepository();
        }

        public async Task<MovieGenres> Add(MovieGenres entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<MovieGenres>> FilteredList(FilteredList<MovieGenres> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<MovieGenres> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<MovieGenres>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<MovieGenres>> Import(List<MovieGenres> entity)
        {
            return await _repo.Import(entity);
        }

        public async Task<ProcessResult> Update(MovieGenres entity)
        {
            return await _repo.Update(entity);
        }
    }
}
