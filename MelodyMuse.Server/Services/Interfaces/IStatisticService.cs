namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IStatisticService
    {
        Task<(string favoriteSongGenre, string favoriteArtistGenre)> GetUserFavoriteGenresAsync(string userId);
    }
}
