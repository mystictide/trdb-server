using trdb.data.Interface.Films.Junctions;
using trdb.data.Repo.Films.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Films;
using trdb.entity.Films.Junctions;

namespace trdb.business.Films.Junctions
{
    public class FilmProductionCompanyJunctionManager : IFilmProductionCompanyJunction
    {
        private readonly IFilmProductionCompanyJunction _repo;
        public FilmProductionCompanyJunctionManager()
        {
            _repo = new FilmProductionCompanyJunctionRepository();
        }

        public async Task<FilmProductionCompanyJunction> Add(FilmProductionCompanyJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<FilmProductionCompanyJunction>> FilteredList(FilteredList<FilmProductionCompanyJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<FilmProductionCompanyJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<FilmProductionCompanyJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<ProductionCompanies>> Manage(List<ProductionCompanies> entity, int FilmID)
        {
            return await _repo.Manage(entity, FilmID);
        }

        public async Task<ProcessResult> Update(FilmProductionCompanyJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
