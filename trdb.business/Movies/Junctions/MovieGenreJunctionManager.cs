using trdb.data.Interface.Movies.Junctions;
using trdb.data.Repo.Movies.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Movies;
using trdb.entity.Movies.Junctions;

namespace trdb.business.Movies.Junctions
{
    public class MovieGenreJunctionManager : IMovieGenreJunction
    {
        private readonly IMovieGenreJunction _repo;
        public MovieGenreJunctionManager()
        {
            _repo = new MovieGenreJunctionRepository();
        }

        public async Task<MovieGenreJunction> Add(MovieGenreJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<MovieGenreJunction>> FilteredList(FilteredList<MovieGenreJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<MovieGenreJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<MovieGenreJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<MovieGenres>> Manage(List<MovieGenres> entity, int MovieID)
        {
            return await _repo.Manage(entity, MovieID);
        }

        public async Task<ProcessResult> Update(MovieGenreJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
