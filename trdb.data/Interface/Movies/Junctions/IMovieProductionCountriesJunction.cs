using trdb.entity.Movies;
using trdb.entity.Movies.Junctions;

namespace trdb.data.Interface.Movies.Junctions
{
    public interface IMovieProductionCountriesJunction : IBaseInterface<MovieProductionCountryJunction>
    {
        Task<List<ProductionCountries>> Manage(List<ProductionCountries> entity, int MovieID);
    }
}
