using trdb.entity.Films;

namespace trdb.data.Interface.Films
{
    public interface IProductionCompanies : IBaseInterface<ProductionCompanies>
    {
        Task<List<ProductionCompanies>> Import(List<ProductionCompanies> entity);
        Task<List<ProductionCompanies>> GetFilmCompanies(int FilmID);
    }
}
