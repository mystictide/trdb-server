using trdb.data.Interface.Movies.Junctions;
using trdb.data.Repo.Movies.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Movies.Junctions;
using trdb.entity.Movies;

namespace trdb.business.Movies.Junctions
{
    internal class MovieCreditsJunctionManager
    {
        private readonly IMovieCreditsJunction _repo;
        public MovieCreditsJunctionManager()
        {
            _repo = new MovieCreditsJunctionRepository();
        }

        public async Task<MovieCreditsJunction> Add(MovieCreditsJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<MovieCreditsJunction>> FilteredList(FilteredList<MovieCreditsJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<MovieCreditsJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<MovieCreditsJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<People>> Manage(List<People> entity, int MovieID)
        {
            return await _repo.Manage(entity, MovieID);
        }

        public async Task<ProcessResult> Update(MovieCreditsJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
