using trdb.data.Interface.Films;
using trdb.data.Repo.Films;
using trdb.entity.Helpers;
using trdb.entity.Films;

namespace trdb.business.Films
{
    public class PeopleManager : IPeople
    {
        private readonly IPeople _repo;
        public PeopleManager()
        {
            _repo = new PeopleRepository();
        }

        public async Task<People> Add(People entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<People>> FilteredList(FilteredList<People> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<People> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<People>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<People>> GetCast(int FilmID)
        {
            return await _repo.GetCast(FilmID);
        }
        public async Task<List<People>> GetCrew(int FilmID)
        {
            return await _repo.GetCrew(FilmID);
        }

        public async Task<List<People>> Import(List<People> entity)
        {
            return await _repo.Import(entity);
        }

        public async Task<ProcessResult> Update(People entity)
        {
            return await _repo.Update(entity);
        }
    }
}
