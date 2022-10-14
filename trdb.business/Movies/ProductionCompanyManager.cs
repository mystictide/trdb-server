using trdb.data.Interface.Movies;
using trdb.data.Repo.Movies;
using trdb.entity.Helpers;
using trdb.entity.Movies;

namespace trdb.business.Movies
{
    public class ProductionCompanyManager : IProductionCompanies
    {
        private readonly IProductionCompanies _repo;
        public ProductionCompanyManager()
        {
            _repo = new ProductionCompaniesRepository();
        }

        public async Task<ProductionCompanies> Add(ProductionCompanies entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<ProductionCompanies>> FilteredList(FilteredList<ProductionCompanies> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<ProductionCompanies> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<ProductionCompanies>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<ProductionCompanies>> Import(List<ProductionCompanies> entity)
        {
            return await _repo.Import(entity);
        }

        public async Task<ProcessResult> Update(ProductionCompanies entity)
        {
            return await _repo.Update(entity);
        }
    }
}
