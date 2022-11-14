using trdb.data.Interface.Films;
using trdb.data.Repo.Films;
using trdb.entity.Helpers;
using trdb.entity.Films;

namespace trdb.business.Films
{
    public class ProductionCountryManager : IProductionCountries
    {
        private readonly IProductionCountries _repo;
        public ProductionCountryManager()
        {
            _repo = new ProductionCountriesRepository();
        }

        public async Task<ProductionCountries> Add(ProductionCountries entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<ProductionCountries>> FilteredList(FilteredList<ProductionCountries> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<ProductionCountries> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<ProductionCountries>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<ProductionCountries>> GetFilmCountries(int FilmID)
        {
            return await _repo.GetFilmCountries(FilmID);
        }

        public async Task<List<ProductionCountries>> Import(List<ProductionCountries> entity)
        {
            return await _repo.Import(entity);
        }

        public async Task<ProcessResult> Update(ProductionCountries entity)
        {
            return await _repo.Update(entity);
        }
    }
}
