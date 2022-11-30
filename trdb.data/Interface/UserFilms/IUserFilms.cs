﻿using trdb.entity.Returns;
using trdb.entity.UserFilms;

namespace trdb.data.Interface.UserFilms
{
    public interface IUserFilms
    {
        Task<UserFilmReturns> GetUserFilmDetails(int ID, int UserID);
        Task<UserFilmReviews> ManageReview(UserFilmReviews entity, int UserID);
        Task<UserFilmRatings> ManageRatings(UserFilmRatings entity, int UserID);
        Task<bool> ToggleWatched(int FilmID, int UserID);
        Task<bool> ToggleWatchlist(int FilmID, int UserID);
        Task<bool> ToggleLike(int FilmID, int UserID);
    }
}
