using trdb.entity.Movies;

namespace trdb.data.Interface.Movies
{
    public interface IPeople : IBaseInterface<People>
    {
        Task<List<People>> Import(List<People> entity);
        Task<List<People>> GetCast(int MovieID);
        Task<List<People>> GetCrew(int MovieID);
    }
}
