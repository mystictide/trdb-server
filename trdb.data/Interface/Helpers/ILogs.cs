using trdb.entity.Helpers;

namespace trdb.data.Interface.Helpers
{
    public interface ILogs
    {
        Task<int> Add(Logs entity);
    }
}
