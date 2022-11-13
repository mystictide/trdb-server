using trdb.data.Interface.UserMovies;
using trdb.data.Repo.User;
using trdb.entity.UserMovies;

namespace trdb.business.Users
{
    public class UserMoviesManager : IUserMovies
    {
        private readonly IUserMovies _repo;
        public UserMoviesManager()
        {
            _repo = new UserMoviesRepository();
        }
        public async Task<UserMovieReviews> ManageReview(UserMovieReviews entity, int UserID)
        {
            return await _repo.ManageReview(entity, UserID);
        }

        public async Task<UserMovieRatings> ManageRatings(UserMovieRatings entity, int UserID)
        {
            return await _repo.ManageRatings(entity, UserID);
        }

        public async Task<bool> ToggleLike(int MovieID, int UserID)
        {
            return await _repo.ToggleLike(MovieID, UserID);
        }

        public async Task<bool> ToggleWatched(int MovieID, int UserID)
        {
            return await _repo.ToggleWatched(MovieID, UserID);
        }

        public async Task<bool> ToggleWatchlist(int MovieID, int UserID)
        {
            return await _repo.ToggleWatchlist(MovieID, UserID);
        }
    }
}
