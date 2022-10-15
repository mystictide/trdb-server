using trdb.data.Interface.Movies.Junctions;
using trdb.data.Repo.Movies.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Movies;
using trdb.entity.Movies.Junctions;

namespace trdb.business.Movies.Junctions
{
    public class MovieProductionCompanyJunctionManager : IMovieProductionCompanyJunction
    {
        private readonly IMovieProductionCompanyJunction _repo;
        public MovieProductionCompanyJunctionManager()
        {
            _repo = new MovieProductionCompanyJunctionRepository();
        }

        public async Task<MovieProductionCompanyJunction> Add(MovieProductionCompanyJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<MovieProductionCompanyJunction>> FilteredList(FilteredList<MovieProductionCompanyJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<MovieProductionCompanyJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<MovieProductionCompanyJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<ProductionCompanies>> Manage(List<ProductionCompanies> entity, int MovieID)
        {
            return await _repo.Manage(entity, MovieID);
        }

        public async Task<ProcessResult> Update(MovieProductionCompanyJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
