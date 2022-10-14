using trdb.entity.Movies;
using trdb.entity.Movies.Junctions;

namespace trdb.data.Interface.Movies.Junctions
{
    public interface IMovieProductionCompanyJunction : IBaseInterface<MovieProductionCompanyJunction>
    {
        Task<List<MovieProductionCompanyJunction>> Manage(List<ProductionCompanies> entity, int MovieID);
    }
}
