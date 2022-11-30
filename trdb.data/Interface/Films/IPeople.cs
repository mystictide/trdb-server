using trdb.entity.Films;
using trdb.entity.Returns;

namespace trdb.data.Interface.Films
{
    public interface IPeople : IBaseInterface<People>
    {
        Task<List<People>> Import(List<People> entity);
        Task<List<People>> GetCast(int FilmID);
        Task<CrewReturn> GetCrew(int FilmID);
    }
}
