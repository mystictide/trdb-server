using trdb.data.Interface.UserFilms;
using trdb.data.Repo.User;
using trdb.entity.UserFilms;

namespace trdb.business.Users
{
    public class UserFilmManager : IUserFilms
    {
        private readonly IUserFilms _repo;
        public UserFilmManager()
        {
            _repo = new UserFilmsRepository();
        }
        public async Task<UserFilmReviews> ManageReview(UserFilmReviews entity, int UserID)
        {
            return await _repo.ManageReview(entity, UserID);
        }

        public async Task<UserFilmRatings> ManageRatings(UserFilmRatings entity, int UserID)
        {
            return await _repo.ManageRatings(entity, UserID);
        }

        public async Task<bool> ToggleLike(int FilmID, int UserID)
        {
            return await _repo.ToggleLike(FilmID, UserID);
        }

        public async Task<bool> ToggleWatched(int FilmID, int UserID)
        {
            return await _repo.ToggleWatched(FilmID, UserID);
        }

        public async Task<bool> ToggleWatchlist(int FilmID, int UserID)
        {
            return await _repo.ToggleWatchlist(FilmID, UserID);
        }
    }
}
