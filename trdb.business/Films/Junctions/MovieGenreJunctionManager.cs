using trdb.data.Interface.Films.Junctions;
using trdb.data.Repo.Films.Junctions;
using trdb.entity.Helpers;
using trdb.entity.Films;
using trdb.entity.Films.Junctions;

namespace trdb.business.Films.Junctions
{
    public class FilmGenreJunctionManager : IFilmGenreJunction
    {
        private readonly IFilmGenreJunction _repo;
        public FilmGenreJunctionManager()
        {
            _repo = new FilmGenreJunctionRepository();
        }

        public async Task<FilmGenreJunction> Add(FilmGenreJunction entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<FilmGenreJunction>> FilteredList(FilteredList<FilmGenreJunction> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<FilmGenreJunction> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<FilmGenreJunction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<FilmGenres>> Manage(List<FilmGenres> entity, int FilmID)
        {
            return await _repo.Manage(entity, FilmID);
        }

        public async Task<ProcessResult> Update(FilmGenreJunction entity)
        {
            return await _repo.Update(entity);
        }
    }
}
