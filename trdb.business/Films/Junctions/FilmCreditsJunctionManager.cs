using trdb.data.Interface.Films.Junctions;
using trdb.data.Repo.Films.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Films.Junctions;
using trdb.entity.Films;

namespace trdb.business.Films.Junctions
{
    internal class FilmCreditsJunctionManager
    {
        private readonly IFilmCreditsJunction _repo;
        public FilmCreditsJunctionManager()
        {
            _repo = new FilmCreditsJunctionRepository();
        }

        public async Task<FilmCreditsJunction> Add(FilmCreditsJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<FilmCreditsJunction>> FilteredList(FilteredList<FilmCreditsJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<FilmCreditsJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<FilmCreditsJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<People>> Manage(List<People> entity, int FilmID)
        {
            return await _repo.Manage(entity, FilmID);
        }

        public async Task<ProcessResult> Update(FilmCreditsJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
