using trdb.data.Interface.Films.Junctions;
using trdb.data.Repo.Films.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Films;
using trdb.entity.Films.Junctions;

namespace trdb.business.Films.Junctions
{
    public class FilmProductionCountryJunctionManager : IFilmProductionCountriesJunction
    {
        private readonly IFilmProductionCountriesJunction _repo;
        public FilmProductionCountryJunctionManager()
        {
            _repo = new FilmProductionCountryJunctionRepository();
        }

        public async Task<FilmProductionCountryJunction> Add(FilmProductionCountryJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<FilmProductionCountryJunction>> FilteredList(FilteredList<FilmProductionCountryJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<FilmProductionCountryJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<FilmProductionCountryJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<ProductionCountries>> Manage(List<ProductionCountries> entity, int FilmID)
        {
            return await _repo.Manage(entity, FilmID);
        }

        public async Task<ProcessResult> Update(FilmProductionCountryJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
