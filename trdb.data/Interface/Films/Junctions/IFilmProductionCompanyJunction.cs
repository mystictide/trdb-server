using trdb.entity.Films;
using trdb.entity.Films.Junctions;

namespace trdb.data.Interface.Films.Junctions
{
    public interface IFilmProductionCompanyJunction : IBaseInterface<FilmProductionCompanyJunction>
    {
        Task<List<ProductionCompanies>> Manage(List<ProductionCompanies> entity, int FilmID);
    }
}
