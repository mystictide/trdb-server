using trdb.entity.UserMovies;

namespace trdb.data.Interface.UserMovies
{
    public interface IUserMovies
    {
        Task<UserMovieReviews> ManageReview(UserMovieReviews entity, int UserID);
        Task<UserMovieRatings> ManageRatings(UserMovieRatings entity, int UserID);
        Task<bool> ToggleWatched(int MovieID, int UserID);
        Task<bool> ToggleWatchlist(int MovieID, int UserID);
        Task<bool> ToggleLike(int MovieID, int UserID);
    }
}
