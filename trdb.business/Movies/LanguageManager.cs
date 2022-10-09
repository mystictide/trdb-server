using trdb.data.Interface.Movies;
using trdb.data.Repo.Movies;
using trdb.entity.Helpers;
using trdb.entity.Movies;

namespace trdb.business.Movies
{
    public class LanguageManager : ILanguages
    {
        private readonly ILanguages _repo;
        public LanguageManager()
        {
            _repo = new LanguagesRepository();
        }

        public async Task<Languages> Add(Languages entity)
        {
            return await _repo.Add(entity);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<FilteredList<Languages>> FilteredList(FilteredList<Languages> request)
        {
            return await _repo.FilteredList(request);
        }

        public async Task<Languages> Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<IEnumerable<Languages>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<ProcessResult> Update(Languages entity)
        {
            return await _repo.Update(entity);
        }
    }
}
