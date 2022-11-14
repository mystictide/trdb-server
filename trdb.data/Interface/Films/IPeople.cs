using trdb.entity.Films;

namespace trdb.data.Interface.Films
{
    public interface IPeople : IBaseInterface<People>
    {
        Task<List<People>> Import(List<People> entity);
        Task<List<People>> GetCast(int FilmID);
        Task<List<People>> GetCrew(int FilmID);
    }
}
