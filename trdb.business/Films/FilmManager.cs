using trdb.business.Films.Junctions;
using trdb.data.Interface.Films;
using trdb.data.Repo.Films;
using trdb.entity.Helpers;

namespace trdb.business.Films
{
    public class FilmManager : IFilms
    {
        private readonly IFilms _repo;
        public FilmManager()
        {
            _repo = new FilmsRepository();
        }

        public async Task<entity.Films.Films> Add(entity.Films.Films entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<entity.Films.Films>> FilteredList(FilteredList<entity.Films.Films> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<entity.Films.Films> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<entity.Films.Films> GetFilmDetails(int? ID, string? title)
        {
            return await _repo.GetFilmDetails(ID, title);
        }

        public async Task<IEnumerable<entity.Films.Films>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<int> GetLatestFilm()
        {
            return await _repo.GetLatestFilm();
        }

        public async Task<entity.Films.Films> Import(entity.Films.Films entity)
        {
            var companies = await new ProductionCompanyManager().Import(entity.Companies);
            var cast = await new PeopleManager().Import(entity.Credits.Cast);
            var crew = await new PeopleManager().Import(entity.Credits.Crew);
            var Film = await _repo.Import(entity);
            Film.Genres = await new FilmGenreJunctionManager().Manage(entity.Genres, Film.ID);
            Film.Languages = await new FilmLanguageJunctionManager().Manage(entity.Languages, Film.ID);
            Film.Companies = await new FilmProductionCompanyJunctionManager().Manage(companies, Film.ID);
            Film.Countries = await new FilmProductionCountryJunctionManager().Manage(entity.Countries, Film.ID);
            Film.Credits.Cast = await new FilmCreditsJunctionManager().Manage(entity.Credits.Cast, Film.ID);
            Film.Credits.Crew = await new FilmCreditsJunctionManager().Manage(entity.Credits.Crew, Film.ID);
            return Film;
        }

        public async Task<ProcessResult> Update(entity.Films.Films entity)
        {
            return await _repo.Update(entity);
        }
    }
}
