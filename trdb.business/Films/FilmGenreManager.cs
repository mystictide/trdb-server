using trdb.data.Interface.Films;
using trdb.data.Repo.FilmGenres;
using trdb.entity.Helpers;
using trdb.entity.Films;

namespace trdb.business.Films
{
    public class FilmGenreManager : IFilmGenres
    {
        private readonly IFilmGenres _repo;
        public FilmGenreManager()
        {
            _repo = new FilmGenresRepository();
        }

        public async Task<FilmGenres> Add(FilmGenres entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<FilmGenres>> FilteredList(FilteredList<FilmGenres> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<FilmGenres> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<FilmGenres>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<FilmGenres>> GetFilmGenres(int FilmID)
        {
            return await _repo.GetFilmGenres(FilmID);
        }

        public async Task<List<FilmGenres>> Import(List<FilmGenres> entity)
        {
            return await _repo.Import(entity);
        }

        public async Task<ProcessResult> Update(FilmGenres entity)
        {
            return await _repo.Update(entity);
        }
    }
}
