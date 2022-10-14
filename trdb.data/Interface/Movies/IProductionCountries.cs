using trdb.entity.Movies;

namespace trdb.data.Interface.Movies
{
    public interface IProductionCountries : IBaseInterface<ProductionCountries>
    {
        Task<List<ProductionCountries>> Import(List<ProductionCountries> entity);
    }
}
