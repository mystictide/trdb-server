using trdb.business.Movies.Junctions;
using trdb.data.Interface.Movies;
using trdb.data.Repo.Movies;
using trdb.entity.Helpers;

namespace trdb.business.Movies
{
    public class MovieManager : IMovies
    {
        private readonly IMovies _repo;
        public MovieManager()
        {
            _repo = new MoviesRepository();
        }

        public async Task<entity.Movies.Movies> Add(entity.Movies.Movies entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<entity.Movies.Movies>> FilteredList(FilteredList<entity.Movies.Movies> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<entity.Movies.Movies> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<entity.Movies.Movies>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<entity.Movies.Movies> Import(entity.Movies.Movies entity)
        {
            var companies = await new ProductionCompanyManager().Import(entity.Companies);
            var movie = await _repo.Import(entity);
            var genreJunk = await new MovieGenreJunctionManager().Manage(entity.Genres, movie.ID);
            var languageJunk = await new MovieLanguageJunctionManager().Manage(entity.Languages, movie.ID);
            var companiesJunk = await new MovieProductionCompanyJunctionManager().Manage(entity.Companies, movie.ID);
            var countriesJunk = await new MovieProductionCountryJunctionManager().Manage(entity.Countries, movie.ID);
            return movie;
        }

        public async Task<ProcessResult> Update(entity.Movies.Movies entity)
        {
            return await _repo.Update(entity);
        }
    }
}
