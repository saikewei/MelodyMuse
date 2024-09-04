namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IStatisticRepository
    {
        Task<(string favoriteSongGenre, string favoriteArtistGenre)> GetUserFavoriteGenresAsync(string userId);
    }
}
