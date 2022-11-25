using trdb.data.Interface.Films.Junctions;
using trdb.data.Repo.Films.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Films;
using trdb.entity.Films.Junctions;

namespace trdb.business.Films.Junctions
{
    public class FilmLanguageJunctionManager : IFilmLanguageJunction
    {
        private readonly IFilmLanguageJunction _repo;
        public FilmLanguageJunctionManager()
        {
            _repo = new FilmLanguageJunctionRepository();
        }

        public async Task<FilmLanguageJunction> Add(FilmLanguageJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<FilmLanguageJunction>> FilteredList(FilteredList<FilmLanguageJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<FilmLanguageJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<FilmLanguageJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<Languages>> Manage(List<Languages> entity, int FilmID)
        {
            return await _repo.Manage(entity, FilmID);
        }

        public async Task<ProcessResult> Update(FilmLanguageJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
