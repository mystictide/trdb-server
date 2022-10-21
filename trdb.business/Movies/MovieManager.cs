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

        public async Task<int> GetLatestMovie()
        {
            return await _repo.GetLatestMovie();
        }

        public async Task<entity.Movies.Movies> Import(entity.Movies.Movies entity)
        {
            var companies = await new ProductionCompanyManager().Import(entity.Companies);
            var cast = await new PeopleManager().Import(entity.Credits.Cast);
            var crew = await new PeopleManager().Import(entity.Credits.Crew);
            var movie = await _repo.Import(entity);
            movie.Genres = await new MovieGenreJunctionManager().Manage(entity.Genres, movie.ID);
            movie.Languages = await new MovieLanguageJunctionManager().Manage(entity.Languages, movie.ID);
            movie.Companies = await new MovieProductionCompanyJunctionManager().Manage(companies, movie.ID);
            movie.Countries = await new MovieProductionCountryJunctionManager().Manage(entity.Countries, movie.ID);
            movie.Credits.Cast = await new MovieCreditsJunctionManager().Manage(entity.Credits.Cast, movie.ID);
            movie.Credits.Crew = await new MovieCreditsJunctionManager().Manage(entity.Credits.Crew, movie.ID);
            return movie;
        }

        public async Task<ProcessResult> Update(entity.Movies.Movies entity)
        {
            return await _repo.Update(entity);
        }
    }
}
