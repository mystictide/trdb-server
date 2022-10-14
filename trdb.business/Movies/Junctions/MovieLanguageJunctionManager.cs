using trdb.data.Interface.Movies.Junctions;
using trdb.data.Repo.Movies.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Movies;
using trdb.entity.Movies.Junctions;

namespace trdb.business.Movies.Junctions
{
    public class MovieLanguageJunctionManager : IMovieLanguageJunction
    {
        private readonly IMovieLanguageJunction _repo;
        public MovieLanguageJunctionManager()
        {
            _repo = new MovieLanguageJunctionRepository();
        }

        public async Task<MovieLanguageJunction> Add(MovieLanguageJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<MovieLanguageJunction>> FilteredList(FilteredList<MovieLanguageJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<MovieLanguageJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<MovieLanguageJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<MovieLanguageJunction>> Manage(List<Languages> entity, int MovieID)
        {
            return await _repo.Manage(entity, MovieID);
        }

        public async Task<ProcessResult> Update(MovieLanguageJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
