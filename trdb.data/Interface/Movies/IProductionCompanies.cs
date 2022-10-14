using trdb.entity.Movies;

namespace trdb.data.Interface.Movies
{
    public interface IProductionCompanies : IBaseInterface<ProductionCompanies>
    {
        Task<List<ProductionCompanies>> Import(List<ProductionCompanies> entity);
    }
}
