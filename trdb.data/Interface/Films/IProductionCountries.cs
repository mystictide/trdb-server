using trdb.entity.Films;

namespace trdb.data.Interface.Films
{
    public interface IProductionCountries : IBaseInterface<ProductionCountries>
    {
        Task<List<ProductionCountries>> Import(List<ProductionCountries> entity);
        Task<List<ProductionCountries>> GetFilmCountries(int FilmID);
    }
}
