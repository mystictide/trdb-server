using trdb.data.Interface.Helpers;
using trdb.data.Repo.Helpers;

namespace trdb.business.Helpers
{
    public class WeeklyManager : IWeekly
    {
        private readonly IWeekly _repo;
        public WeeklyManager()
        {
            _repo = new WeeklyRepository();
        }

        public async Task<entity.Movies.Movies> Manage()
        {
            return await _repo.Manage();
        }
    }
}
