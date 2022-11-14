using trdb.entity.Films;
using trdb.entity.Films.Junctions;

namespace trdb.data.Interface.Films.Junctions
{
    public interface IFilmProductionCountriesJunction : IBaseInterface<FilmProductionCountryJunction>
    {
        Task<List<ProductionCountries>> Manage(List<ProductionCountries> entity, int FilmID);
    }
}
