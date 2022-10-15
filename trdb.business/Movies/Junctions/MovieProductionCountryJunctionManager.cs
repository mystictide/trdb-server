using trdb.data.Interface.Movies.Junctions;
using trdb.data.Repo.Movies.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Movies;
using trdb.entity.Movies.Junctions;

namespace trdb.business.Movies.Junctions
{
    public class MovieProductionCountryJunctionManager : IMovieProductionCountriesJunction
    {
        private readonly IMovieProductionCountriesJunction _repo;
        public MovieProductionCountryJunctionManager()
        {
            _repo = new MovieProductionCountryJunctionRepository();
        }

        public async Task<MovieProductionCountryJunction> Add(MovieProductionCountryJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<MovieProductionCountryJunction>> FilteredList(FilteredList<MovieProductionCountryJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<MovieProductionCountryJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<MovieProductionCountryJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<ProductionCountries>> Manage(List<ProductionCountries> entity, int MovieID)
        {
            return await _repo.Manage(entity, MovieID);
        }

        public async Task<ProcessResult> Update(MovieProductionCountryJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
